# 미완료 영역 세부 구현 현황

구현 54/81 (66.7%). 전체 항목 102/117 (87.2%)와 분모가 다른 보조 지표입니다.

실행 검증은 별도이며 소스 구현 상태를 뜻합니다. 분모는 버전 내 고정합니다.

이번 집계: 완료 +0개 / 진행 중 전환 0개 / 감소 0개. 현재 진행 중 25개. 최초 기준선: False

| 영역 | 구현/전체 | 진행 중 | 구현율 |
|---|---:|---:|---:|
| 구조화 참조·상대 이름을 포함한 모든 잘라내기 참조 변환 | 1/3 | 2 | 33.3% |
| 패턴·그라데이션 색 필터 | 4/4 | 0 | 100.0% |
| 테이블 스타일·시스템 팔레트·아이콘 기반 필터 | 4/4 | 0 | 100.0% |
| 대체 달력과 전체 로케일 필터 | 2/4 | 2 | 50.0% |
| 상대·동적·구조화 이름과 전체 의존성 호환 | 6/8 | 2 | 75.0% |
| 기존 수식 함수의 미지원 인수·모드 해소 | 4/6 | 2 | 66.7% |
| 현재 카탈로그 밖 수식 지원 범위 확정·구현 | 6/7 | 1 | 85.7% |
| 조건부 차등 서식 전체 속성 | 4/7 | 3 | 57.1% |
| 상대·혼합 참조 및 퇴화 임계값의 조건부 서식 | 3/4 | 0 | 75.0% |
| x14 데이터 막대와 아이콘 집합 | 2/2 | 0 | 100.0% |
| 전체 로케일·혼합 표시 형식 | 1/3 | 2 | 33.3% |
| 회전된 부모 그룹·일반 아핀/전단 변환 | 5/6 | 1 | 83.3% |
| 그룹 생성·해제 및 그룹 내부 차트 | 4/6 | 2 | 66.7% |
| 복잡한·3D·부드러운 곡선 차트 | 1/3 | 2 | 33.3% |
| 계열별 차트 종류·축 자유 변경 | 3/5 | 2 | 60.0% |
| 전체 개체 스타일·부모 확장 메타데이터 편집 | 2/4 | 2 | 50.0% |
| 외부 데이터의 표시·숨김 완전 지원 | 0/2 | 2 | 0.0% |
| 전체 실제 문서의 개체·관계·스타일 편집 보존 | 2/3 | 0 | 66.7% |

## 세부 상태

- [x] E2.13.S01 구조화 참조 내부 A1 형태 열 이름 보호 (implemented)
- [ ] E2.13.S02 상대 이름 잘라내기 변환 (partial)
- [ ] E2.13.S03 테이블 구조 변경과 잘라내기 참조 통합 (partial)
- [x] E4.07.S01 단색 패턴 색 필터 (implemented)
- [x] E4.07.S02 동일 색 그라데이션 필터 (implemented)
- [x] E4.07.S03 다색 패턴 색 필터 (implemented)
- [x] E4.07.S04 다색 그라데이션 색 필터 (implemented)
- [x] E4.08.S01 사용자 테이블 스타일 색 읽기 (implemented)
- [x] E4.08.S02 시스템 팔레트 64·65 기본값 (implemented)
- [x] E4.08.S03 실제 시스템 테마 팔레트 감지 (implemented)
- [x] E4.08.S04 아이콘 기반 필터 (implemented)
- [x] E4.09.S01 달력 식별자 읽기·보존 (implemented)
- [x] E4.09.S02 Gregorian 비교 경로 (implemented)
- [ ] E4.09.S03 대체 달력 변환 (partial)
- [ ] E4.09.S04 전체 로케일 날짜 필터 (partial)
- [x] E5.17.S01 절대 정의 이름 (implemented)
- [x] E5.17.S02 정규 테이블 열·구간 참조 (implemented)
- [x] E5.17.S03 테이블 특수 구역 선택자 (implemented)
- [x] E5.17.S04 현재 행 @·#This Row 참조 (implemented)
- [x] E5.17.S05 테이블 내부 비정규 참조 탐색 (implemented)
- [ ] E5.17.S06 상대 정의 이름 (partial)
- [ ] E5.17.S07 동적 이름 전체 의존성 (partial)
- [x] E5.17.S08 이름·LAMBDA 호출 위치의 현재 행 전달 (implemented)
- [x] E5.18.S01 XLOOKUP 다중 검색 값 (implemented)
- [x] E5.18.S02 XLOOKUP·XMATCH 생략 인수 기본값 (implemented)
- [x] E5.18.S03 REDUCE 생략 초기값 (implemented)
- [x] E5.18.S04 SCAN 생략 초기값 (implemented)
- [ ] E5.18.S05 MAP 서로 다른 배열 크기 (partial)
- [ ] E5.18.S06 조회 다중 반환 열과 다중 검색 결합 (partial)
- [x] E5.19.S01 LAMBDA·LET (implemented)
- [x] E5.19.S02 MAP·BYROW·BYCOL·MAKEARRAY (implemented)
- [x] E5.19.S03 REDUCE·SCAN·ISOMITTED (implemented)
- [x] E5.19.S04 텍스트 분리·결합·추출 함수 (implemented)
- [x] E5.19.S05 배열 적층·투영·평탄화 함수 (implemented)
- [x] E5.19.S06 현재 카탈로그 밖 함수 범위 확정 (implemented)
- [ ] E5.19.S07 확정된 추가 함수 구현 (partial)
- [x] E6.09.S01 차등 채우기 패턴·그라데이션 (implemented)
- [x] E6.09.S02 차등 테두리·대각선 (implemented)
- [x] E6.09.S03 차등 글꼴 이름 (implemented)
- [x] E6.09.S04 차등 기본 정렬·줄바꿈 (implemented)
- [ ] E6.09.S05 고급 정렬·회전·들여쓰기 (partial)
- [ ] E6.09.S06 글꼴 스킴·문자 집합·효과 (partial)
- [ ] E6.09.S07 보호 속성 (partial)
- [x] E6.10.S01 일반 조건부 수식 상대·혼합 참조 변환 (implemented)
- [x] E6.10.S02 레거시 임계 수식 구조 변경 참조 변환 (implemented)
- [x] E6.10.S03 퇴화 임계값 전체 처리 (implemented)
- [ ] E6.10.S04 레거시 상대 임계 수식 문맥 (remaining)
- [x] E6.11.S01 x14 데이터 막대 전체 속성 (implemented)
- [x] E6.11.S02 x14 아이콘 집합 (implemented)
- [x] E6.12.S01 기본 숫자·날짜 표시 형식 (implemented)
- [ ] E6.12.S02 전체 로케일 표시 형식 (partial)
- [ ] E6.12.S03 혼합 텍스트 구역·색 지시자 (partial)
- [x] E7.15.S01 그룹 아핀 미리보기 (implemented)
- [x] E7.15.S02 그룹 자식 위치·크기 편집 (implemented)
- [x] E7.15.S03 아핀 개체 복사·잘라내기 (implemented)
- [x] E7.15.S04 부모 그룹 프레임 편집 (implemented)
- [ ] E7.15.S05 일반 아핀 그룹 해제 (partial)
- [x] E7.15.S06 방향을 따르는 편집 핸들 (implemented)
- [x] E7.16.S01 최상위 개체 그룹 생성 (implemented)
- [x] E7.16.S02 단순 그룹 해제 (implemented)
- [x] E7.16.S03 그룹 내부 차트 미리보기 (implemented)
- [x] E7.16.S04 그룹 내부 차트 편집 (implemented)
- [ ] E7.16.S05 중첩·회전 그룹 해제 (partial)
- [ ] E7.16.S06 그룹 효과·연결자 전체 처리 (partial)
- [x] E7.17.S01 부드러운 곡선 차트 (implemented)
- [ ] E7.17.S02 3D 차트 (partial)
- [ ] E7.17.S03 추가 복잡 차트 종류 (partial)
- [x] E7.18.S01 계열별 막대·선·영역 종류 (implemented)
- [x] E7.18.S02 계열별 주·보조 축 (implemented)
- [x] E7.18.S03 레이어별 누적·백분율 누적 (implemented)
- [ ] E7.18.S04 나머지 차트 종류 계열 전환 (partial)
- [ ] E7.18.S05 전체 축·스타일 조합 (partial)
- [x] E7.19.S01 개체 기본 위치·크기·텍스트 편집 (implemented)
- [x] E7.19.S02 변경하지 않은 개체 XML 유지 (implemented)
- [ ] E7.19.S03 개체 전체 스타일 편집 (partial)
- [ ] E7.19.S04 부모 확장 메타데이터 편집 (partial)
- [ ] E7.20.S01 외부 데이터 새로 고침 (partial)
- [ ] E7.20.S02 외부 데이터 표시·숨김 완전 반영 (partial)
- [x] E8.06.S01 ZIP 원본 기반 변경 부분 저장 (implemented)
- [x] E8.06.S02 실제 문서 전체 개체 관계 편집 보존 (implemented)
- [ ] E8.06.S03 실제 Excel 왕복 보존 승인 (remaining)
