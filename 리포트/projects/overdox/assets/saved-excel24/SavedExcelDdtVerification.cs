using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DesignDataTable;
using DesignDataTable.Editor;
using Overdox.OriginalData;
using UnityEditor;
using UnityEngine;

namespace Overdox.Editor
{
    /// <summary>
    /// 디스크에 저장한 Excel 검증 사본을 실제 DDT 리더·행 변환·암호화 경로로 검증한다.
    /// </summary>
    public static class SavedExcelDdtVerification
    {
        private const string OUTPUT_FOLDER = "Docs/Verification/ExcelSavedRoundTrip";
        private const int WEAPON_ID = 10750;

        /// <summary>
        /// 원본 자산을 변경하지 않고 사본 수식 계산값과 저장한 DDT의 재로딩 결과를 비교한다.
        /// </summary>
        [MenuItem("OVERDOX/Data/Verify saved Excel through DDT")]
        public static void Verify()
        {
            if (EditorApplication.isPlaying)
            {
                throw new InvalidOperationException("재생을 종료한 뒤 실행하세요.");
            }

            string variantPath = OUTPUT_FOLDER + "/OverdoxGameData-scale110.xlsx";
            string originalHash = HashFile(ExcelGameDataCompiler.WorkbookPath);
            var book = WorkbookReader.Read(variantPath, false);
            var gear = book.Sheets.Single(sheet => sheet.Name == "Gear");
            var calculation = new WorkbookCalculation(book);
            // 메모리 값을 주입하지 않는다. 제작 컴파일러와 같은 행 매핑이 저장된 파일을 해석한다.
            var rows = ExcelGameDataCompiler.ReadTable(gear, calculation, typeof(Gear));
            float computedBase = rows.Cast<Gear>().Single(row => row.Id == WEAPON_ID).DamageBase;
            var balance = book.Sheets.Single(sheet => sheet.Name == "Balance");
            int previewRow = Enumerable.Range(4, balance.RowCount - 4).Single(row => balance.Cell(row, 0)?.Value == WEAPON_ID.ToString(CultureInfo.InvariantCulture));
            float computedPreview = Convert.ToSingle(calculation.Value("Balance", previewRow, 7), CultureInfo.InvariantCulture);
            if (computedBase != 132 || computedPreview != 215)
            {
                throw new InvalidDataException($"저장 XLSX 계산 불일치: base={computedBase}, preview={computedPreview}");
            }

            using var stream = new MemoryStream();
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                writer.Write(rows.Count);
                foreach (var row in rows)
                {
                    row.Write(writer);
                }
            }
            string binaryPath = OUTPUT_FOLDER + "/saved-Gear.bytes";
            File.WriteAllBytes(binaryPath, DataLoader.Encrypt(GearCatalog.BINARY_KEY, stream.ToArray()));
            // 저장 이전 버퍼가 아닌 실제 파일을 다시 읽어 바이너리 저장·로딩 경계까지 확인한다.
            var decoded = DataLoader.ReadBinaryDictionary<Gear>(File.ReadAllBytes(binaryPath), GearCatalog.BINARY_KEY);
            bool isOriginalUnchanged = originalHash == HashFile(ExcelGameDataCompiler.WorkbookPath);
            if (decoded.Count != 38 || decoded[WEAPON_ID].DamageBase != 132 || !isOriginalUnchanged)
            {
                throw new InvalidDataException("저장 DDT 검증 또는 원본 보존 확인에 실패했습니다.");
            }
            var result = new VerificationResult
            {
                recordedAt = DateTime.UtcNow.ToString("O"),
                workbookHash = HashFile(variantPath),
                binaryHash = HashFile(binaryPath),
                originalHash = originalHash,
                weaponId = WEAPON_ID,
                rows = decoded.Count,
                computedBase = computedBase,
                computedPreview = computedPreview,
                decodedBase = decoded[WEAPON_ID].DamageBase,
                isOriginalUnchanged = isOriginalUnchanged,
                scope = "Saved XLSX read, production DDT formula engine and row mapping, encrypted file write and DataLoader file reload; Windows display not verified"
            };
            File.WriteAllText(OUTPUT_FOLDER + "/unity-saved-ddt.json", JsonUtility.ToJson(result, true));
            Debug.Log("OVERDOX_SAVED_EXCEL_DDT " + JsonUtility.ToJson(result));
        }

        /// <summary>
        /// 입력 파일의 SHA256을 계산해 사본 식별과 원본 보존 비교에 사용한다.
        /// </summary>
        private static string HashFile(string path)
        {
            using var hash = SHA256.Create();
            return BitConverter.ToString(hash.ComputeHash(File.ReadAllBytes(path))).Replace("-", "");
        }

        /// <summary>
        /// 저장 경계별 검증 결과와 실제 검증 범위를 JSON으로 남긴다.
        /// </summary>
        [Serializable]
        private sealed class VerificationResult
        {
            public string recordedAt, workbookHash, binaryHash, originalHash, scope;
            public int weaponId, rows;
            public float computedBase, computedPreview, decodedBase;
            public bool isOriginalUnchanged;
        }
    }
}
