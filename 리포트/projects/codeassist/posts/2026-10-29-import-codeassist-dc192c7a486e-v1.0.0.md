---
title: 108개 전체 근거 대조
date: 2026-10-17T16:49:47.678Z
version: v1.0.0
status: 기존 기록 이전
project: codeassist
type: 작업 결과
category: 이전 기록
summary: 108개 전체 근거 대조 — 당시 작업 내용과 검증 범위를 원문 기준으로 정리한 보관 기록입니다.
source: ../assets/imports/docs/all-108-review-20261017.html
source_sha256: f4389820516b1502c06c18067a42e1088c8e05d5121ced46416fdfa7ddaab1d8
record: archived
format: legacy-v1
---

> 보관 기록입니다. 아래 결과는 원문 작성 시점의 내용이며, 이번 편집에서 제품 테스트를 다시 실행하지 않았습니다.

[2026-10-18 최신: 실행 오류 수정·24시나리오 양쪽 실행 결과](../assets/imports/docs/automated-parity-runner.html)

[2026-10-17 최신 실행 대조 중간 저장·미완료 항목](../assets/imports/docs/runtime-stop-20261017.html)

108개 전체 근거 대조 

## 108개 전체 근거 대조

108개 행의 문서 요구·현재 코드·제한을 검토했습니다. 108개 실행 대조 완료 또는 동기화율 100%를 뜻하지 않습니다. 전체 유사도는 미확인입니다.

부분 명세: 108/108. CA 자체 테스트: 267/267. 완전 동등성 인증: 0/108. 후보 테스트 연결은 해당 기능 전체 통과를 뜻하지 않습니다.

[실제 VA·CA 화면 대조](../assets/imports/docs/live-comparison-20261017.html) · [공식 146개 문서 목차](../assets/imports/docs/va-documentation-comparison.html) · 전체 원본 JSON

VA-F001 · Find by Context

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/navigation/find-by-context)

- 같은 이름이어도 다른 scope 객체는 제외하며 활성 파일의 다음/이전 참조로 이동

- 커서 또는 선택 심볼에서 실행; 전체 파일 간 검색은 Find References에 해당

### CA의 현재 범위·차이

활성 파일 문맥 참조 이동; 전체 의미 분석 미완료

### 현재 코드 근거 후보

 CodeAssist.Core/ContextReferenceNavigation.cs:38

```
    }
    public static class ContextReferenceNavigation
    {
        public static SelectionRange Move(string text,int caret,bool forward)
        {
            if(text==null||caret<0||caret>text.Length)return null;
            var word=IdentifierParser.At(text,caret);if(word.Length==0)return null;
```

 CodeAssist.Core/ContextReferenceNavigation.cs:47

```
            while(start>0&&(char.IsLetterOrDigit(text[start-1])||text[start-1]=='_'))start--;
            var context=new CppReferenceContext(text);
            if(start<0||start>=context.Code.Length||context.Code.Substring(start,word.Length)!=word)return null;
            var matches=context.References(word,context.DeclarationAt(word,start));
            if(matches.Count==0)return null;
            if(forward){foreach(var offset in matches)if(offset>start)return new SelectionRange(offset,word.Length);return new SelectionRange(matches[0],word.Length);}
            for(var i=matches.Count-1;i>=0;i--)if(matches[i]<start)return new SelectionRange(matches[i],word.Length);
```

 CodeAssist.Core/ContextReferenceNavigation.cs:48

```
            var context=new CppReferenceContext(text);
            if(start<0||start>=context.Code.Length||context.Code.Substring(start,word.Length)!=word)return null;
            var matches=context.References(word,context.DeclarationAt(word,start));
            if(matches.Count==0)return null;
            if(forward){foreach(var offset in matches)if(offset>start)return new SelectionRange(offset,word.Length);return new SelectionRange(matches[0],word.Length);}
            for(var i=matches.Count-1;i>=0;i--)if(matches[i]<start)return new SelectionRange(matches[i],word.Length);
            return new SelectionRange(matches[matches.Count-1],word.Length);
```

 CodeAssist.Extension/ToolbarCommands.cs:57

```
                var id = item.Key;
                if (!(service?.FindCommand(new CommandID(GoToSymbolCommand.CommandSet,id)) is OleMenuCommand command)) continue;
                void UpdateLabel(object sender, EventArgs args)
                {
                    ThreadHelper.ThrowIfNotOnUIThread();
                    command.Text = UiCommandLabels.Get(id,StartupUi.Preferences.UiLanguage);
                }
```

 CodeAssist.Extension/ToolbarCommands.cs:77

```
                var names=new System.Collections.Generic.Dictionary<int,string>{
                    {0x141,"Settings"},{0x103,"OpenFile"},{0x104,"FindSymbol"},{0x102,"FindReferences"},
                    {0x150,"PreviousContext"},{0x151,"NextContext"},{0x117,"Snippet"},{0x152,"SpellCheck"},
                    {0x105,"CorrespondingFile"},{0x153,"SkipBreakpoints"},{0x154,"ThreadBreakpoints"},{0x140,"Welcome"}};
                foreach(Microsoft.VisualStudio.CommandBars.CommandBarControl control in bars["CodeAssist"].Controls)
                {
                    if(!names.TryGetValue(control.Id,out var key)) {
```

 CodeAssist.Extension/ToolbarCommands.cs:78

```
                    {0x141,"Settings"},{0x103,"OpenFile"},{0x104,"FindSymbol"},{0x102,"FindReferences"},
                    {0x150,"PreviousContext"},{0x151,"NextContext"},{0x117,"Snippet"},{0x152,"SpellCheck"},
                    {0x105,"CorrespondingFile"},{0x153,"SkipBreakpoints"},{0x154,"ThreadBreakpoints"},{0x140,"Welcome"}};
                foreach(Microsoft.VisualStudio.CommandBars.CommandBarControl control in bars["CodeAssist"].Controls)
                {
                    if(!names.TryGetValue(control.Id,out var key)) {
                        key=control.Tag as string;
```

### 추가 실행 대조

- 동명 지역/멤버/중첩 scope 비교

- 첫/마지막 참조의 경계 및 선택 방향 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F002 · Find References

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/navigation/find-references)

- 창별 D/R/W 필터 및 적용 가능한 필터만 표시

- D/R/W는 새 검색마다 해당 결과 전체로 시작

- 프로젝트 검색 범위는 이전 검색 상태 유지; 지역 변수는 파일 범위 고정

- 단일 행의 복수 참조는 복수 결과로 표시

- 상속/override·주석/문자열·생성/구성·scope 결과 필터

- 결과 내 Ctrl+F·F3·Shift+F3 탐색

### CA의 현재 범위·차이

CA 결과 창은 목록과 결과 수를 제공하며 VA의 D/R/W 필터·프로젝트 트리는 제공하지 않습니다.

### 현재 코드 근거 후보

 CodeAssist.Extension/ReferenceResults.cs:18

```
    {
        private readonly ListBox list = new ListBox();
        private readonly TextBlock title = new TextBlock { Margin = new System.Windows.Thickness(5) };
        private AsyncPackage package;
        internal ReferencePane()
        {
            var panel = new DockPanel(); DockPanel.SetDock(title, Dock.Top); panel.Children.Add(title); panel.Children.Add(list); Content = panel;
```

 CodeAssist.Extension/FindReferencesCommand.cs:13

```
{
    internal sealed class FindReferencesCommand
    {
        public const int CommandId = 0x0102;
        private static readonly Guid OutputPaneGuid = new Guid("5725d377-6839-49a5-b9e2-67694bb4594f");
        private readonly AsyncPackage package;
        private readonly IntelliSenseSquiggleController controller;
```

 CodeAssist.Extension/FindReferencesCommand.cs:22

```
        private FindReferencesCommand(AsyncPackage package, IntelliSenseSquiggleController controller, ProjectIndexService indexService, OleMenuCommandService commandService)
        {
            this.package = package;
            this.controller = controller;
            this.indexService = indexService;
            commandService.AddCommand(new OleMenuCommand(Execute, new CommandID(GoToSymbolCommand.CommandSet, CommandId)));
```

 CodeAssist.Extension/FindReferencesCommand.cs:34

```
            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null) _ = new FindReferencesCommand(package, controller, indexService, commandService);
        }

        private void Execute(object sender, EventArgs e)
        {
            pendingSearch?.Cancel(); pendingSearch?.Dispose();
```

### 추가 실행 대조

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F003 · Find Symbol in Solution

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/navigation/find-symbol-in-solution)

- 드롭다운은 대소문자 무시; 대화상자는 다중 AND·제외·점 경계 필터와 선택적 uppercase case matching

- 기본 선택은 prefix 우선, 없으면 첫 contains; 심볼 이름 알파벳 정렬은 열 클릭으로 변경되지 않음

- solution/system 범위 및 classes/struct/namespaces 제한; 선택 초기화 후 Ctrl+Z로 이전 필터 복원

- 지역 변수 제외; 선언 위치·언어별 포함 심볼 규칙 구분

### CA의 현재 범위·차이

동일 fixture의 19개 심볼 결과는 실제 일치했습니다. 순서·이름 표기·필터 UI는 다릅니다.

### 현재 코드 근거 후보

 CodeAssist.Extension/NavigationCommands.cs:214

```
                await package.JoinableTaskFactory.SwitchToMainThreadAsync();
                var tagPicker = new NavigationPicker("CodeAssist: Hashtags", tags, package.JoinableTaskFactory);
                new System.Windows.Interop.WindowInteropHelper(tagPicker).Owner = (IntPtr)dte.MainWindow.HWnd;
                if (tagPicker.ShowDialog() == true) Open(dte, tagPicker.Selected);
                return;
            }
            if (command == 0x0111)
```

 CodeAssist.Extension/NavigationCommands.cs:231

```
                }).ToArray();
                var includePicker = new NavigationPicker("CodeAssist: Include Files", includes, package.JoinableTaskFactory);
                new System.Windows.Interop.WindowInteropHelper(includePicker).Owner = (IntPtr)dte.MainWindow.HWnd;
                if (includePicker.ShowDialog() == true) Open(dte, includePicker.Selected);
                return;
            }
            var symbol = selection == null ? "" : IdentifierParser.At(selection.ActivePoint.CreateEditPoint().GetLines(selection.ActivePoint.Line, selection.ActivePoint.Line + 1), selection.ActivePoint.LineCharOffset - 1);
```

 CodeAssist.Extension/NavigationCommands.cs:263

```
            var title = command == 0x0103 ? "CodeAssist: 파일 찾기" : command == 0x0105 ? "CodeAssist: 대응 파일" : command == 0x0106 ? "CodeAssist: 현재 파일 메서드" : command == 0x0107 ? "CodeAssist: 관련 심볼 후보" : "CodeAssist: 심볼 찾기";
            var picker = new NavigationPicker(title, entries, package.JoinableTaskFactory, indexes.ProjectRoot, command == 0x0104);
            new System.Windows.Interop.WindowInteropHelper(picker).Owner = (IntPtr)dte.MainWindow.HWnd;
            if (picker.ShowDialog() == true && picker.Selected != null) { Open(dte, picker.Selected); if(picker.FindReferencesRequested) StartupUi.Execute(0x0102); }
        }
        private static bool IsImplementation(string path) => new[]{".cpp",".cxx",".cc",".c"}.Contains(Path.GetExtension(path).ToLowerInvariant());
        private static NavigationEntry Entry(IndexedSymbol item) => new NavigationEntry((item.OwnerType.Length > 0 ? item.OwnerType + "::" : "") +
```

### 추가 실행 대조

- prefix/contains·대소문자·부정/경계 조합 비교

- 선택 초기화·이전 필터 복원 및 system 범위 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F004 · GoTo Implementation

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/navigation/goto-implementation)

- Alt+G: 목적지 하나면 직접 이동, 복수면 메뉴; 구현 항목은 {...} 표시

- 활성 project/solution 범위 설정과 system symbol의 임시 범위 확대

- 주석 scope가 없으면 동명 후보 메뉴; hashtag와 프로젝트 밖 파일 지원

- C/C++에서 VA 후보가 없고 IntelliSense가 켜져 있으면 IDE GoTo Definition 호출

- 구현 검색은 선언 디렉터리와 ../src·../source 및 구성 경로 사용

### CA의 현재 범위·차이

clangd 정의 조회 후 인덱스 또는 VS 명령으로 이동합니다. 모든 VA 언어·문맥에서의 목적지 일치는 미검증입니다.

### 현재 코드 근거 후보

 CodeAssist.Extension/GoToSymbolCommand.cs:93

```
            var semanticText = semanticDocument?.StartPoint.CreateEditPoint().GetText(semanticDocument.EndPoint);
            var compilerResults = await CompilerSemanticService.Instance.FindAsync("textDocument/definition", solutionDirectory,
                dte.FullName, dte.ActiveDocument.FullName, semanticText, selection.ActivePoint.Line, selection.ActivePoint.LineCharOffset, package.DisposalToken);
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();
            var results = compilerResults != null && compilerResults.Length > 0 ? compilerResults.Select(item =>
                new SourceSymbol(symbolName, item.File, item.Line, item.Column, SymbolKind.Unknown)).ToArray() : indexService?.Index.FindDefinitions(symbolName);
            if (results == null || results.Count == 0)
```

### 추가 실행 대조

- 단일/복수/overload 및 주석 scope 비교

- project 제한·system 예외·후보 없음 fallback 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F005 · GoTo Member

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/navigation/goto-member)

- 현재 type/variable 또는 enclosing structure 공백에서 해당 member 탐색; enum/interface/namespace/struct/typedef 지원

- base member 포함 옵션·다중문자/negative/점prefix 필터·복사·Find References·type별 정렬

### CA의 현재 범위·차이

인덱스 기반 멤버 선택·이동; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/EditorFeatureCommands.cs:33

```
                    try{await owner.ExecuteAsync(captured);}catch(Exception error){await package.JoinableTaskFactory.SwitchToMainThreadAsync();MessageBox.Show(error.Message,"CodeAssist",MessageBoxButton.OK,MessageBoxImage.Warning);}
                }).FileAndForget("CodeAssist/EditorFeature"),new CommandID(GoToSymbolCommand.CommandSet,id)));
            }
        }
        private async System.Threading.Tasks.Task ExecuteAsync(int id)
        {
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();
```

 CodeAssist.Extension/EditorFeatureCommands.cs:68

```
                var type=word.Length==0?CppExpressionContext.FindEnclosingFunctionOwner(text,caret):new CppReferenceContext(text).ResolveVariableType(word,caret)??indexes.Index.ResolveVariableType(word)??word;
                var entries=indexes.Index.CompleteMembers(type,"",2000).Where(s=>File.Exists(s.FilePath)).Select(s=>new NavigationEntry((s.OwnerType.Length>0?s.OwnerType+"::":"")+(s.Signature.Length>0?s.Signature:s.Name),s.FilePath,s.Line,s.Column)).ToArray();
                var picker=new NavigationPicker("CodeAssist: Go To Member",entries,package.JoinableTaskFactory);new System.Windows.Interop.WindowInteropHelper(picker).Owner=(IntPtr)dte.MainWindow.HWnd;
                if(picker.ShowDialog()==true&&picker.Selected!=null){dte.ItemOperations.OpenFile(picker.Selected.File,EnvDTE.Constants.vsViewKindTextView);(dte.ActiveDocument.Selection as EnvDTE.TextSelection)?.MoveToLineAndOffset(picker.Selected.Line,picker.Selected.Column,false);}return;
            }
            if(id==0x175){if(selected.Length==0)return;SnippetInsertion.Show(view,file,indexes.ProjectRoot,(IntPtr)dte.MainWindow.HWnd,null,dte.Solution?.FullName,true);return;}
            if(id==0x176)
```

 CodeAssist.Extension/EditorFeatureCommands.cs:69

```
                var entries=indexes.Index.CompleteMembers(type,"",2000).Where(s=>File.Exists(s.FilePath)).Select(s=>new NavigationEntry((s.OwnerType.Length>0?s.OwnerType+"::":"")+(s.Signature.Length>0?s.Signature:s.Name),s.FilePath,s.Line,s.Column)).ToArray();
                var picker=new NavigationPicker("CodeAssist: Go To Member",entries,package.JoinableTaskFactory);new System.Windows.Interop.WindowInteropHelper(picker).Owner=(IntPtr)dte.MainWindow.HWnd;
                if(picker.ShowDialog()==true&&picker.Selected!=null){dte.ItemOperations.OpenFile(picker.Selected.File,EnvDTE.Constants.vsViewKindTextView);(dte.ActiveDocument.Selection as EnvDTE.TextSelection)?.MoveToLineAndOffset(picker.Selected.Line,picker.Selected.Column,false);}return;
            }
            if(id==0x175){if(selected.Length==0)return;SnippetInsertion.Show(view,file,indexes.ProjectRoot,(IntPtr)dte.MainWindow.HWnd,null,dte.Solution?.FullName,true);return;}
            if(id==0x176)
            {
```

### 추가 실행 대조

- 각 구조종류·inherited toggle·동명member·type/name sort

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F006 · GoTo Related

실행 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/navigation/goto-related)

- Shift+Alt+G: 심볼 종류별 base/derived·override·생성자·type/typedef·선언/정의·header 목적지

- 상속 관계는 하위 메뉴; 가능한 파일/줄/namespace 표시

- include에서는 관련 명령 및 해당 header를 include하는 파일; 주석에서는 가장 가능성 높은 심볼 선택

- 오버로드 목적지 순서와 call signature matching은 옵션 영향

### CA의 현재 범위·차이

기존 장부에 구체적인 구현 범위 설명 누락. 후보 파일 존재만 확인함.

### 현재 코드 근거 후보

 CodeAssist.Extension/NavigationCommands.cs:41

```
                service.AddCommand(new OleMenuCommand((s, e) => package.JoinableTaskFactory.RunAsync(() => commands.ExecuteAsync(command))
                    .FileAndForget("CodeAssist/Navigation"), new CommandID(GoToSymbolCommand.CommandSet, id)));
            }
        }

        private async System.Threading.Tasks.Task ExecuteAsync(int command)
        {
```

 CodeAssist.Extension/NavigationCommands.cs:256

```
                var owner = text == null || selection == null ? null : CppExpressionContext.FindEnclosingFunctionOwner(text, selection.ActivePoint.AbsoluteCharOffset - 1);
                entries = await System.Threading.Tasks.Task.Run(() => (command == 0x0107 ? indexes.Index.RelatedSymbols(symbol, owner) :
                    indexes.Index.SearchSymbols("", limit: int.MaxValue))
                    .GroupBy(item=>item.Kind+"|"+item.OwnerType+"|"+item.Name+"|"+item.Signature+"|"+item.ValueType)
                    .Select(group=>group.OrderBy(item=>!string.IsNullOrEmpty(indexes.ProjectRoot)&&item.FilePath.StartsWith(indexes.ProjectRoot.TrimEnd(Path.DirectorySeparatorChar)+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)?0:1).ThenBy(item=>IsImplementation(item.FilePath)?0:1).First()).Select(Entry).ToArray());
            }
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();
```

 CodeAssist.Extension/NavigationCommands.cs:333

```
            var buttons=new StackPanel {Orientation=Orientation.Horizontal,HorizontalAlignment=HorizontalAlignment.Right};DockPanel.SetDock(buttons,Dock.Right);footer.Children.Add(buttons);
            var go=ProductTheme.Button("Goto",()=>{if(Selected!=null)DialogResult=true;},true);go.IsDefault=true;buttons.Children.Add(go);
            if(symbols) buttons.Children.Add(ProductTheme.Button("Find References",()=>{if(Selected!=null){FindReferencesRequested=true;DialogResult=true;}}));
            var cancel=ProductTheme.Button("Cancel",()=>DialogResult=false);cancel.IsCancel=true;buttons.Children.Add(cancel);
            var brand=new StackPanel {Orientation=Orientation.Horizontal,VerticalAlignment=VerticalAlignment.Center};
            var brandFile=Path.Combine(Path.GetDirectoryName(typeof(NavigationPicker).Assembly.Location),"Resources","CodeAssist16.png");
            if(File.Exists(brandFile))brand.Children.Add(new Image {Source=new System.Windows.Media.Imaging.BitmapImage(new Uri(brandFile)),Width=16,Height=16,Margin=new Thickness(0,0,5,0)});
```

### 추가 실행 대조

- 다이아몬드 상속·override·overload 메뉴 비교

- include 역방향 의존성·주석 후보 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F007 · Highlight Current Line

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- caret 줄 표시; VA Current Line IDE 색 설정 사용

- 배경·색상/frame·점선·실선·margin까지 ray 형식

### CA의 현재 범위·차이

VS 현재 줄 강조 옵션 연결; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/EditorGuides.cs:40

```
            if(!map.At(position,out partner)){position=caret-1;if(!map.At(position,out partner))yield break;}
            var p=StartupUi.Preferences;if(partner<0&&!p.HighlightMismatchingBraces||partner>=0&&!p.HighlightMatchingBraces)yield break;
            var tag=new TextMarkerTag(partner<0?"CodeAssist Mismatched Brace":"CodeAssist Matched Brace");
            foreach(var offset in partner<0?new[]{position}:new[]{position,partner}){var span=new SnapshotSpan(snapshot,offset,1);foreach(var requested in spans)if(requested.IntersectsWith(span)){yield return new TagSpan<TextMarkerTag>(span,tag);break;}}
        }
    }
    [Export(typeof(IWpfTextViewCreationListener))][ContentType("C/C++")][TextViewRole(PredefinedTextViewRoles.Document)]
```

 CodeAssist.Extension/EditorGuides.cs:60

```
            if(view.IsClosed)return;layer.RemoveAllAdornments();var p=StartupUi.Preferences;
            if(view.Options.GetOptionValue(DefaultWpfViewOptions.EnableHighlightCurrentLineId)!=p.HighlightCurrentLine)view.Options.SetOptionValue(DefaultWpfViewOptions.EnableHighlightCurrentLineId,p.HighlightCurrentLine);
            if(!p.ColumnIndicatorEnabled||view.ViewportHeight<=0)return;
            var x=view.FormattedLineSource.BaseIndentation+view.FormattedLineSource.ColumnWidth*p.ColumnIndicator-view.ViewportLeft;
            if(x<0||x>view.ViewportWidth)return;
            var line=new Line {X1=x,X2=x,Y1=0,Y2=view.ViewportHeight,Stroke=Brushes.Gray,Opacity=.5,StrokeThickness=1,IsHitTestVisible=false};
            layer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative,null,null,line,null);
```

 CodeAssist.Extension/EditorGuides.cs:62

```
            if(!p.ColumnIndicatorEnabled||view.ViewportHeight<=0)return;
            var x=view.FormattedLineSource.BaseIndentation+view.FormattedLineSource.ColumnWidth*p.ColumnIndicator-view.ViewportLeft;
            if(x<0||x>view.ViewportWidth)return;
            var line=new Line {X1=x,X2=x,Y1=0,Y2=view.ViewportHeight,Stroke=Brushes.Gray,Opacity=.5,StrokeThickness=1,IsHitTestVisible=false};
            layer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative,null,null,line,null);
        }
        private void Closed(object sender,EventArgs e){view.LayoutChanged-=Layout;view.Closed-=Closed;StartupUi.SettingsApplied-=Update;}
```

### 추가 실행 대조

- theme·포커스·색·표시방식 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F008 · List Methods in File

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/navigation/list-methods-in-file)

- Alt+M: 편집에 따라 갱신; 기본 파일 등장 순서 및 상단 navigation bar

- 다중 AND·negative·점 경계·comma OR 필터

- class/method/macro 확장 method/member/struct/enum/define/event/property/region 항목 옵션

- namespace·parameter 표시 및 정렬 옵션; ESC/외부 클릭 닫힘

### CA의 현재 범위·차이

CA는 현재 파일 함수 5개를 별도 검색창에 표시합니다. VA는 탐색 바 드롭다운입니다.

### 현재 코드 근거 후보

 CodeAssist.Extension/NavigationCommands.cs:251

```
                entries = await System.Threading.Tasks.Task.Run(() => PersistentSymbolIndex.ReadDocumentSymbols(file, text)
                    .Where(item => item.Kind == SymbolKind.Function).Select(Entry).ToArray());
            }
            else
            {
                var owner = text == null || selection == null ? null : CppExpressionContext.FindEnclosingFunctionOwner(text, selection.ActivePoint.AbsoluteCharOffset - 1);
                entries = await System.Threading.Tasks.Task.Run(() => (command == 0x0107 ? indexes.Index.RelatedSymbols(symbol, owner) :
```

### 추가 실행 대조

- macro 메서드·namespace·parameter 표시 조합 비교

- 편집 revision 갱신 및 ESC/포커스 종료 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F009 · Move Scope

실행 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/navigation/move-scope)

- Alt+Down/Up: enclosing brace/parenthesis 바깥 scope의 시작/끝 이동

- global scope에 도달하면 메서드 간 이동

### CA의 현재 범위·차이

기존 장부에 구체적인 구현 범위 설명 누락. 후보 파일 존재만 확인함.

### 현재 코드 근거 후보

 CodeAssist.Extension/NavigationCommands.cs:87

```
                                view.Selection.Select(new SnapshotSpan(updated, change.Start, change.Text.Length), false);
                                view.Caret.MoveTo(new SnapshotPoint(updated, change.Start + change.Text.Length));
                            }
                        }
                        else if (command == 0x011d)
                        {
                            if (!view.TextBuffer.ContentType.IsOfType("C/C++")) return;
```

 CodeAssist.Extension/NavigationCommands.cs:150

```
                            view.Selection.Select(new SnapshotSpan(view.TextSnapshot, range.Start, range.Length), false);
                            view.Caret.MoveTo(new SnapshotPoint(view.TextSnapshot, range.End));
                            view.ViewScroller.EnsureSpanVisible(new SnapshotSpan(view.TextSnapshot, range.Start, range.Length));
                        }
                    }
                }
                return;
```

 CodeAssist.Extension/NavigationCommands.cs:187

```
                var position = selection.ActivePoint.AbsoluteCharOffset - 1;
                var target = CppScopeNavigation.Move(text, position, command == 0x010d);
                if (target >= 0) selection.MoveToAbsoluteOffset(target + 1, false);
                return;
            }
            controller?.Refresh();
            var projectMarker=controller?.UnrealProjectPath;
```

### 추가 실행 대조

- 괄호와 brace 혼합 중첩 및 global 이동 비교

- 문자열/주석 내 delimiter와 불완전 코드 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F010 · Navigate Back and Forward

실행 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/navigation/navigate-back-and-forward)

- 단순 caret 이동 waypoint는 제외; Alt+Left/Right는 다른 명령 바인딩 영향을 받음

- Alt+Right의 Edit.CompleteWord 바인딩 제거 조건; waypoint 종류의 icon 표는 별도 확인 필요

### CA의 현재 범위·차이

기존 장부에 구체적인 구현 범위 설명 누락. 후보 파일 존재만 확인함.

### 현재 코드 근거 후보

 CodeAssist.Extension/NavigationCommands.cs:167

```
            {
                var native = new[] { "View.NavigateBackward", "View.NavigateForward", "Refactor.Rename", "View.QuickActions", "View.QuickActions" }[command - 0x0108];
                try
                {
                    if (!dte.Commands.Item(native).IsAvailable) { ShowUnavailable(native); return; }
                    dte.ExecuteCommand(native);
                }
```

 CodeAssist.Extension/NavigationCommands.cs:317

```
            public object Convert(object[] values,Type targetType,object parameter,System.Globalization.CultureInfo culture) => ProductTheme.Palette.Convert(values.Length>1?values[1]:null,targetType,values.Length>0?values[0]:null,culture);
            public object[] ConvertBack(object value,Type[] types,object parameter,System.Globalization.CultureInfo culture)=>throw new NotSupportedException();
        }
        public NavigationPicker(string title, NavigationEntry[] entries, Microsoft.VisualStudio.Threading.JoinableTaskFactory tasks, string solutionRoot=null, bool symbols=false)
        {
            this.entries = entries; Title=title; Width=886; Height=396; MinWidth=640; MinHeight=300; WindowStartupLocation=WindowStartupLocation.CenterOwner;
            ProductTheme.Apply(this); FontSize=12;
```

 CodeAssist.Extension/NavigationCommands.cs:342

```
            var template=new DataTemplate(typeof(NavigationEntry));var label=new FrameworkElementFactory(typeof(TextBlock));label.SetBinding(TextBlock.TextProperty,new Binding("Name"));
            var colors=new MultiBinding {Converter=new SymbolInkConverter()};colors.Bindings.Add(new Binding("Kind"));colors.Bindings.Add(new Binding("Background"){RelativeSource=new RelativeSource(RelativeSourceMode.FindAncestor,typeof(ListView),1)});label.SetBinding(TextBlock.ForegroundProperty,colors);var icon=new FrameworkElementFactory(typeof(Microsoft.VisualStudio.Imaging.CrispImage));icon.SetValue(FrameworkElement.WidthProperty,16.0);icon.SetValue(FrameworkElement.HeightProperty,16.0);icon.SetValue(FrameworkElement.MarginProperty,new Thickness(0,0,5,0));icon.SetBinding(Microsoft.VisualStudio.Imaging.CrispImage.MonikerProperty,new Binding("Icon"));icon.SetValue(DockPanel.DockProperty,Dock.Left);var symbolCell=new FrameworkElementFactory(typeof(DockPanel));symbolCell.AppendChild(icon);symbolCell.AppendChild(label);template.VisualTree=symbolCell;
            columns.Columns.Add(new GridViewColumn {Header=UiText.Get(symbols?"Symbol":"File",StartupUi.Preferences.UiLanguage),Width=290,CellTemplate=template});
            columns.Columns.Add(new GridViewColumn {Header=UiText.Get(symbols?"Definition":"Path",StartupUi.Preferences.UiLanguage),Width=510,DisplayMemberBinding=new Binding("Definition")});
            var rows=new Style(typeof(ListViewItem));rows.Setters.Add(new Setter(Control.PaddingProperty,new Thickness(3,2,3,2)));rows.Setters.Add(new Setter(FrameworkElement.ToolTipProperty,new Binding("File")));
            rows.Setters.Add(new Setter(Control.BackgroundProperty,Brushes.Transparent));
            rows.Setters.Add(new Setter(Control.ForegroundProperty,new System.Windows.DynamicResourceExtension(EnvironmentColors.ToolWindowTextBrushKey)));
```

### 추가 실행 대조

- 편집/파일전환/Find reference waypoint와 cursor column 복원

- keyboard conflict·history branch·닫힌문서

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F011 · Open Corresponding File

실행 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/navigation/open-corresponding-file)

- Alt+O: 대응 하나면 직접 열고 복수면 선택 메뉴; C++ 및 C# design/codebehind 등 관계 지원

- 현재 directory 우선; 없으면 project·parent·inc/include 검색; type 미확정이면 유사prefix 후보

### CA의 현재 범위·차이

기존 장부에 구체적인 구현 범위 설명 누락. 후보 파일 존재만 확인함.

### 현재 코드 근거 후보

 CodeAssist.Extension/NavigationCommands.cs:187

```
                var position = selection.ActivePoint.AbsoluteCharOffset - 1;
                var target = CppScopeNavigation.Move(text, position, command == 0x010d);
                if (target >= 0) selection.MoveToAbsoluteOffset(target + 1, false);
                return;
            }
            controller?.Refresh();
            var projectMarker=controller?.UnrealProjectPath;
```

 CodeAssist.Extension/NavigationCommands.cs:216

```
                new System.Windows.Interop.WindowInteropHelper(tagPicker).Owner = (IntPtr)dte.MainWindow.HWnd;
                if (tagPicker.ShowDialog() == true) Open(dte, tagPicker.Selected);
                return;
            }
            if (command == 0x0111)
            {
                if (text == null || file == null) return;
```

 CodeAssist.Extension/NavigationCommands.cs:233

```
                new System.Windows.Interop.WindowInteropHelper(includePicker).Owner = (IntPtr)dte.MainWindow.HWnd;
                if (includePicker.ShowDialog() == true) Open(dte, includePicker.Selected);
                return;
            }
            var symbol = selection == null ? "" : IdentifierParser.At(selection.ActivePoint.CreateEditPoint().GetLines(selection.ActivePoint.Line, selection.ActivePoint.Line + 1), selection.ActivePoint.LineCharOffset - 1);
            NavigationEntry[] entries;
            if (command == 0x0103 || command == 0x0105)
```

### 추가 실행 대조

- 다중 대응·project외·이름접두사·경로동명 후보

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F012 · Open File in Solution

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/navigation/open-file-in-solution)

- Shift+Alt+O: 다중 선택과 Ctrl+A; 범위·IDE key scheme에 따라 기본 shortcut scope 차이

- AND·negative·점 경계·comma OR·path 필터·fuzzy 옵션; base filename prefix 선택 우선

- 숨겨도 적용되는 persistent filter와 Ctrl+R 해제

- solution·directory·private/shared system·external·open 파일 공급원 및 project 중복 결합 옵션

- line/column qualifier는 형식별 조건; 기본 base filename 정렬 및 열별 reverse 정렬

- ESC/Stop은 non-solution 로딩만 중단하고 solution 파일은 유지

### CA의 현재 범위·차이

실행 2개 검색 사례와 과거 관찰 10개 결과 집합은 일치합니다. Solution only 및 표시 UI는 다릅니다.

### 현재 코드 근거 후보

 CodeAssist.Extension/NavigationCommands.cs:201

```
            {
                var files = indexes.Index.SourceFiles().Concat(file != null && text != null ? new[] { file } : Array.Empty<string>()).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
                var activeFile = file; var activeText = text;
                var tags = await System.Threading.Tasks.Task.Run(() => files.SelectMany(path =>
                {
                    try
                    {
```

 CodeAssist.Extension/NavigationCommands.cs:222

```
                if (text == null || file == null) return;
                var files = indexes.Index.SourceFiles();
                var includes = CppDocumentNavigation.Includes(text).SelectMany(include =>
                {
                    var relative = include.Name.Replace('/', Path.DirectorySeparatorChar);
                    var local = Path.Combine(Path.GetDirectoryName(file), relative);
                    return files.Where(path => path.EndsWith(Path.DirectorySeparatorChar + relative, StringComparison.OrdinalIgnoreCase))
```

 CodeAssist.Extension/NavigationCommands.cs:240

```
            {
                entries = await System.Threading.Tasks.Task.Run(() => indexes.Index.SourceFiles().Select(path => new NavigationEntry(Path.GetFileName(path), path, 1, 1)).ToArray());
                if (command == 0x0105)
                {
                    entries = entries.Where(entry => !string.Equals(entry.File, file, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(Path.GetFileNameWithoutExtension(entry.File), Path.GetFileNameWithoutExtension(file), StringComparison.OrdinalIgnoreCase)).ToArray();
                    if (entries.Length == 1) { Open(dte, entries[0]); return; }
```

### 추가 실행 대조

- 다중 project 중복/결합·공급원 변경 및 취소 비교

- persistent filter 숨김/해제·line/column 조건 비교

- fuzzy·case·prefix·다중 선택 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F013 · VA Hashtags

실행 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/navigation/va-hashtags)

- 주석 내 공백없는 tag; 대소문자 무시·다중tag·Goto exact/창 substring 검색

- 자동갱신·tag/file 그룹·AND/negative/OR필터·Shift+Alt+H/F8/Shift+F8

- 숨김은 tag/file/recursive directory/project 범위; tool window만 영향·Goto/완성 가능

- solution .vaHashtags 외부변경은 IDE재시작; read-only면 규칙 저장 불가

### CA의 현재 범위·차이

기존 장부에 구체적인 구현 범위 설명 누락. 후보 파일 존재만 확인함.

### 현재 코드 근거 후보

 CodeAssist.Extension/NavigationCommands.cs:208

```
                        var contents = string.Equals(path, activeFile, StringComparison.OrdinalIgnoreCase) && activeText != null ? activeText : File.ReadAllText(path);
                        return CppDocumentNavigation.Hashtags(contents).Select(tag => new NavigationEntry(tag.Name, path, tag.Line, 1)).ToArray();
                    }
                    catch (IOException) { return Array.Empty<NavigationEntry>(); }
                    catch (UnauthorizedAccessException) { return Array.Empty<NavigationEntry>(); }
                }).OrderBy(item => item.Name).ThenBy(item => item.File).ToArray());
                await package.JoinableTaskFactory.SwitchToMainThreadAsync();
```

 CodeAssist.Extension/NavigationCommands.cs:214

```
                await package.JoinableTaskFactory.SwitchToMainThreadAsync();
                var tagPicker = new NavigationPicker("CodeAssist: Hashtags", tags, package.JoinableTaskFactory);
                new System.Windows.Interop.WindowInteropHelper(tagPicker).Owner = (IntPtr)dte.MainWindow.HWnd;
                if (tagPicker.ShowDialog() == true) Open(dte, tagPicker.Selected);
                return;
            }
            if (command == 0x0111)
```

### 추가 실행 대조

- 주석/전처리·case·편집갱신·숨김범위

- 외부변경·read-only·solution/global 전환

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F014 · VA Navigation Bar

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/navigation/va-navigation-bar)

- source창 상단/하단·표시 옵션; context/method list/scope arrows/definition/Go 구성

- Go 기본은 implementation이며 related로 변경 가능; namespace 표시 문맥메뉴

- 공식 현재 문서의 다중 project 전환은2630 가용성 별도 확인

### CA의 현재 범위·차이

경로와 메서드 탐색; Context/Definition 필드 전체 미완료

### 현재 코드 근거 후보

 CodeAssist.Extension/ContextBar.cs:25

```
        [Import] internal ProjectIndexService Indexes=null;
        public IWpfTextViewMargin CreateMargin(IWpfTextViewHost host,IWpfTextViewMargin parent)
        {
            Documents.TryGetTextDocument(host.TextView.TextDataModel.DocumentBuffer,out var document);
            return new ContextBarMargin(host.TextView,document?.FilePath,Indexes);
        }
    }
```

### 추가 실행 대조

- 문서·theme·namespace·Go설정·project 전환

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F015 · Access to Refactoring Commands

실행 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/refactoring/access-to-refactoring-and-code-generation)

- Shift+Alt+Q는 커서·선택 문맥에 맞는 가능한 명령만 표시

- 적용 가능성에 따라 명령 순서 결정

- include·symbol·whitespace 문맥을 구분

- 편집기·Outline·VA View에서 명령을 제공

### CA의 현재 범위·차이

기존 장부에 구체적인 구현 범위 설명 누락. 후보 파일 존재만 확인함.

### 현재 코드 근거 후보

 CodeAssist.Extension/CodeAssistCommands.vsct:7

```
  <Commands package="guidCodeAssistPackage">
    <Menus>
      <Menu guid="guidCodeAssistCommandSet" id="CodeAssistToolsMenu" priority="0x0100" type="Menu"><Parent guid="guidCodeAssistCommandSet" id="CodeAssistToolsMenuGroup" /><Strings><ButtonText>CodeAssist</ButtonText></Strings></Menu>
      <Menu guid="guidCodeAssistCommandSet" id="CodeAssistContextMenu" priority="0x0700" type="Menu"><Parent guid="guidSHLMainMenu" id="IDM_VS_CTXT_CODEWIN" /><Strings><ButtonText>CodeAssist</ButtonText></Strings></Menu>
      <Menu guid="guidCodeAssistCommandSet" id="CodeAssistToolbar" priority="0x0100" type="Toolbar">
        <CommandFlag>DefaultDocked</CommandFlag><Strings><ButtonText>CodeAssist</ButtonText><CommandName>CodeAssist</CommandName></Strings>
```

 CodeAssist.Extension/CodeAssistCommands.vsct:9

```
    <Menus>
      <Menu guid="guidCodeAssistCommandSet" id="CodeAssistToolsMenu" priority="0x0100" type="Menu"><Parent guid="guidCodeAssistCommandSet" id="CodeAssistToolsMenuGroup" /><Strings><ButtonText>CodeAssist</ButtonText></Strings></Menu>
      <Menu guid="guidCodeAssistCommandSet" id="CodeAssistContextMenu" priority="0x0700" type="Menu"><Parent guid="guidSHLMainMenu" id="IDM_VS_CTXT_CODEWIN" /><Strings><ButtonText>CodeAssist</ButtonText></Strings></Menu>
      <Menu guid="guidCodeAssistCommandSet" id="CodeAssistToolbar" priority="0x0100" type="Toolbar">
        <CommandFlag>DefaultDocked</CommandFlag><Strings><ButtonText>CodeAssist</ButtonText><CommandName>CodeAssist</CommandName></Strings>
      </Menu>
    </Menus>
```

 CodeAssist.Extension/CodeAssistCommands.vsct:10

```
      <Menu guid="guidCodeAssistCommandSet" id="CodeAssistToolsMenu" priority="0x0100" type="Menu"><Parent guid="guidCodeAssistCommandSet" id="CodeAssistToolsMenuGroup" /><Strings><ButtonText>CodeAssist</ButtonText></Strings></Menu>
      <Menu guid="guidCodeAssistCommandSet" id="CodeAssistContextMenu" priority="0x0700" type="Menu"><Parent guid="guidSHLMainMenu" id="IDM_VS_CTXT_CODEWIN" /><Strings><ButtonText>CodeAssist</ButtonText></Strings></Menu>
      <Menu guid="guidCodeAssistCommandSet" id="CodeAssistToolbar" priority="0x0100" type="Toolbar">
        <CommandFlag>DefaultDocked</CommandFlag><Strings><ButtonText>CodeAssist</ButtonText><CommandName>CodeAssist</CommandName></Strings>
      </Menu>
    </Menus>
    <Groups>
```

### 추가 실행 대조

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F016 · Change Signature

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/refactoring/change-signature)

- 함수 이름 위치에서 선언·정의·참조 및 호출부 변경; 다중 파일 Undo 지원

- 인수 이름 변경은 선언/정의, 인수 추가는 호출부까지 변경

- 기본 인수를 서명에 추가하면 기존 호출부를 변경하지 않음; 추가 인수 호출값 기본은 TODO

- 모호한 변경은 OK 비활성화; 이름 변경과 재정렬은 구분

- C++ 가시성 변경은 지원하지 않음; 문서는 overload·system-directory 제한을 명시

- 프로젝트 검색 범위는 Find References·Rename과 공유

### CA의 현재 범위·차이

VA는 시그니처 각 부분 변경을 제공하지만 CA 명령은 순서 변경만 받습니다. 완전 일치가 아닙니다.

### 현재 코드 근거 후보

 CodeAssist.Extension/SemanticRefactoringCommands.cs:47

```
            {
                var input=EditorFeatureCommands.Prompt(dte,"New parameter order (one-based, e.g. 2,1; reorder only)","2,1");if(input==null)return;
                var order=input.Split(',').Select(v=>int.Parse(v.Trim(),System.Globalization.CultureInfo.InvariantCulture)-1).ToArray();
                var declarations=Locations(await Query("textDocument/declaration",active,caret)).Concat(Locations(await Query("textDocument/definition",active,caret))).ToArray();
                if(declarations.Length==0)throw new NotSupportedException("The compiler did not resolve a callable declaration.");
                var references=Locations(await Query("textDocument/references",active,caret,new JObject {["context"]=new JObject {["includeDeclaration"]=true}})).Concat(declarations).ToArray();
                var declarationKeys=new HashSet<string>(StringComparer.OrdinalIgnoreCase);
```

### 추가 실행 대조

- 기본 인수 추가와 필수 인수 추가의 호출부 차이

- 여러 인수 이름 변경+재정렬 모호성

- 파생 클래스·외부 헤더·선택 호출부·Undo

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F017 · Convert Between Pointer and Instance

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/refactoring/convert-between-pointer-and-instance)

- 지역 변수 선언에서 pointer/instance 전환 및 참조의 dot/arrow·dereference/address 수정

- pointer→instance는 선택한 참조를 변환하고 delete 참조 제거

- instance→pointer는 raw/unique/shared/custom 생성 선택; 기본 auto

- 지원 불가 문맥에서는 안내

### CA의 현재 범위·차이

지역 변수의 멤버 접근 전용 포인터↔인스턴스 변환; 주소 유출·delete·포인터 연산 거부; VA 동등성 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/RefactoringCommands.cs:38

```
                case 0x18b:
                    result=CppDocumentTransforms.ConvertLocalPointer(snapshot.GetText(),start,length);start=0;length=snapshot.Length;title="Convert Local Pointer / Instance";break;
                case 0x18c:
                    if(selected.Length==0)throw new ArgumentException("Select statements to extract.");var signature=Ask("New free-function signature (use reference parameters for outputs)","void Extracted()");if(signature==null)return;
                    result=CppDocumentTransforms.ExtractGlobalMethod(snapshot.GetText(),start,length,signature);start=0;length=snapshot.Length;title="Extract Method";break;
                case 0x180:result=CppRefactoring.SimplifyInstance(source);title="Simplify Instance Declaration";break;
                case 0x181:
```

 CodeAssist.Extension/RefactoringCommands.cs:42

```
                    result=CppDocumentTransforms.ExtractGlobalMethod(snapshot.GetText(),start,length,signature);start=0;length=snapshot.Length;title="Extract Method";break;
                case 0x180:result=CppRefactoring.SimplifyInstance(source);title="Simplify Instance Declaration";break;
                case 0x181:
                    var property=Ask("Accessor name suffix","Value");if(property==null)return;result=CppRefactoring.EncapsulateField(source,property,nl);title="Encapsulate Field (declaration and accessors)";break;
                case 0x182:
                    var member=Ask("Member declaration","void NewMethod();");if(member==null)return;
                    if(!member.TrimEnd().EndsWith(";",StringComparison.Ordinal))throw new ArgumentException("A member declaration must end with a semicolon.");
```

### 추가 실행 대조

- new/delete와 address/dereference 혼합

- raw/unique/shared/custom 및 explicit type

- 지원 불가 변수·escape하는 참조

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F018 · Encapsulate Field

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/refactoring/encapsulate-field)

- C++ 선언 위치에서 getter/setter 생성; 이름 공백이면 해당 accessor 생략

- 필드 가시성 조정; private/protected 필드의 accessor는 기본 public

- 참조 선택 범위를 조절해 읽기/쓰기를 accessor로 변경

- 접두사 제거·대문자화 및 스니펫 GeneratedPropertyName으로 accessor 이름 구성

- C++ accessor는 선언 헤더에 배치; 배열/char pointer는 수동 보완이 필요할 수 있음

### CA의 현재 범위·차이

CA 함수는 필드 선언과 getter/setter 문자열만 생성합니다. VA의 프로젝트 참조 갱신과 같지 않습니다.

### 현재 코드 근거 후보

 CodeAssist.Core/CppRefactoring.cs:81

```
        }
        public static string EncapsulateField(string declaration,string propertyName,string newline="\n")
        {
            var m=Match(declaration??"",@"^\s*(?<type>(?:const\s+)?[A-Za-z_]\w*(?:::[A-Za-z_]\w*)*(?:\s*[*&])?)\s+(?<field>[A-Za-z_]\w*)\s*;\s*$");
            if(!m.Success||!QualifiedIdentifier(propertyName)||propertyName.Contains("::"))return null;
            var type=m.Groups["type"].Value;var field=m.Groups["field"].Value;
            return "private:"+newline+"    "+declaration.Trim()+newline+"public:"+newline+"    "+type+" Get"+propertyName+"() const { return "+field+"; }"+newline+(type.StartsWith("const ",StringComparison.Ordinal)?"":"    void Set"+propertyName+"("+type+" value) { "+field+" = value; }"+newline);
```

 CodeAssist.Extension/RefactoringCommands.cs:44

```
                case 0x181:
                    var property=Ask("Accessor name suffix","Value");if(property==null)return;result=CppRefactoring.EncapsulateField(source,property,nl);title="Encapsulate Field (declaration and accessors)";break;
                case 0x182:
                    var member=Ask("Member declaration","void NewMethod();");if(member==null)return;
                    if(!member.TrimEnd().EndsWith(";",StringComparison.Ordinal))throw new ArgumentException("A member declaration must end with a semicolon.");
                    result=member+nl;start=line.Start;length=0;title="Add Member at Current Line";break;
                case 0x183:
```

### 추가 실행 대조

- m_nItemsCount·hWindow·밑줄 이름

- getter/setter 한쪽 생략·복합 참조

- 배열/char pointer 제한과 선택 참조 범위

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F019 · Extract Method

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/refactoring/extract-method)

- 선택 코드와 주변 문맥에서 인수/반환형을 추론하고 원래 위치를 호출로 대체

- 전역/멤버와 새 함수 내부 로컬은 인수에서 제외

- 읽기값은 값 전달, 쓰기값과 dot로 참조되는 객체는 참조 전달

- 단일 변경 값은 반환값으로 원래 함수에서 대입할 수 있음

### CA의 현재 범위·차이

전역 void 함수로 선택 문장 추출; 입력·출력 매개변수는 사용자 지정, 클래스·템플릿·제어 이전 제외; VA 동등성 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/RefactoringCommands.cs:40

```
                case 0x18c:
                    if(selected.Length==0)throw new ArgumentException("Select statements to extract.");var signature=Ask("New free-function signature (use reference parameters for outputs)","void Extracted()");if(signature==null)return;
                    result=CppDocumentTransforms.ExtractGlobalMethod(snapshot.GetText(),start,length,signature);start=0;length=snapshot.Length;title="Extract Method";break;
                case 0x180:result=CppRefactoring.SimplifyInstance(source);title="Simplify Instance Declaration";break;
                case 0x181:
                    var property=Ask("Accessor name suffix","Value");if(property==null)return;result=CppRefactoring.EncapsulateField(source,property,nl);title="Encapsulate Field (declaration and accessors)";break;
                case 0x182:
```

 CodeAssist.Extension/RefactoringCommands.cs:41

```
                    if(selected.Length==0)throw new ArgumentException("Select statements to extract.");var signature=Ask("New free-function signature (use reference parameters for outputs)","void Extracted()");if(signature==null)return;
                    result=CppDocumentTransforms.ExtractGlobalMethod(snapshot.GetText(),start,length,signature);start=0;length=snapshot.Length;title="Extract Method";break;
                case 0x180:result=CppRefactoring.SimplifyInstance(source);title="Simplify Instance Declaration";break;
                case 0x181:
                    var property=Ask("Accessor name suffix","Value");if(property==null)return;result=CppRefactoring.EncapsulateField(source,property,nl);title="Encapsulate Field (declaration and accessors)";break;
                case 0x182:
                    var member=Ask("Member declaration","void NewMethod();");if(member==null)return;
```

 CodeAssist.Extension/RefactoringCommands.cs:46

```
                case 0x182:
                    var member=Ask("Member declaration","void NewMethod();");if(member==null)return;
                    if(!member.TrimEnd().EndsWith(";",StringComparison.Ordinal))throw new ArgumentException("A member declaration must end with a semicolon.");
                    result=member+nl;start=line.Start;length=0;title="Add Member at Current Line";break;
                case 0x183:
                    var name=Ask("New member name","NewMember");if(name==null)return;var similar=CppRefactoring.SimilarMember(source,name);if(similar!=null)result=source+nl+similar;title="Add Similar Member";break;
                case 0x184:
```

### 추가 실행 대조

- read/write 혼합·멤버·로컬·단일 반환값

- 단일문/다중문·early return·매크로 선택 경계

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F020 · Introduce Variable

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/refactoring/introduce-variable)

- 표현식의 가장 안쪽 scope에 변수 선언 후 선택 표현식 교체

- 반복 교체는 선택 이후의 현재 scope 후보에 한정

- 필요하면 한 줄 if/else/while에 braces 추가

- 빈 공간에서도 함수 반환값 capture에 이용 가능

### CA의 현재 범위·차이

clangd 변수 추출 변경 미리보기; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CompilerActions.cs:19

```
    {
        private sealed class ActionItem {internal JObject Action;public override string ToString()=>(string)Action["title"];}
        internal static async Task<bool> TryAsync(AsyncPackage package,EnvDTE80.DTE2 dte,bool quickFix=false,bool toolWindow=false)
        {
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();
            var file=dte.ActiveDocument?.FullName;if(string.IsNullOrEmpty(file))return false;
            var root=Path.GetDirectoryName(file);
```

### 추가 실행 대조

- 동일 표현식의 선택 전/후와 다른 scope

- 단일문 while/if·부작용 표현식

- 반환값 capture와 residual parentheses

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F021 · Move Implementation to Header File

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/refactoring/move-implementation-to-header-file-class-declaration)

- free function는 header, member는 class내부; 기존declaration 대체·없어도 생성

- declaration의 preprocessor/default값 보존·parameter이름은 implementation 사용

- 연관성이 충분한 선행주석 이동·이동구현 선택 유지·생성snippet 사용

### CA의 현재 범위·차이

clangd 소스→헤더 구현 이동 미리보기; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CompilerActions.cs:19

```
    {
        private sealed class ActionItem {internal JObject Action;public override string ToString()=>(string)Action["title"];}
        internal static async Task<bool> TryAsync(AsyncPackage package,EnvDTE80.DTE2 dte,bool quickFix=false,bool toolWindow=false)
        {
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();
            var file=dte.ActiveDocument?.FullName;if(string.IsNullOrEmpty(file))return false;
            var root=Path.GetDirectoryName(file);
```

### 추가 실행 대조

- 선언없음·default이름차이·전처리·주석·선택복원

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F022 · Move Implementation to Source File

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/refactoring/move-implementation-to-source-file)

- header에 declaration 남김; class/overload 인접배치·CreateImplementation snippet 사용

- 기본 인접배치 또는EOF 옵션; inline template는 먼저 class밖 header로 이동하고 다음 실행에 source로 이동

- 이동 후 VA history로 source/header 왕복

### CA의 현재 범위·차이

clangd 헤더→소스 구현 이동 미리보기; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CompilerActions.cs:44

```
            if(actions==null||actions.Length==0)return false;
            var list=new ListBox {ItemsSource=actions,SelectedIndex=0,Margin=new Thickness(14)};
            var rootPanel=new DockPanel();ProductTheme.Apply(rootPanel);rootPanel.Children.Add(list);
            var dialog=new Window {Title="CodeAssist: Compiler Refactorings",Content=rootPanel,Width=650,Height=400,WindowStartupLocation=WindowStartupLocation.CenterOwner};
            new System.Windows.Interop.WindowInteropHelper(dialog).Owner=(IntPtr)dte.MainWindow.HWnd;
            dialog.PreviewKeyDown+=(s,e)=>{if(e.Key==System.Windows.Input.Key.Escape)dialog.DialogResult=false;if(e.Key==System.Windows.Input.Key.Enter&&list.SelectedItem!=null)dialog.DialogResult=true;};
            list.MouseDoubleClick+=(s,e)=>{if(!toolWindow&&list.SelectedItem!=null)dialog.DialogResult=true;};
```

### 추가 실행 대조

- template 두단계·overload·EOF·snippet·Undo

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F023 · Move Method Implementations to Source File

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/refactoring/move-method-implementations-to-source-file)

- class이름에서 모든method 이동 command·선택dialog

- nested class 선택 시 두번째dialog; template/배치 규칙은 단일move와 공유

### CA의 현재 범위·차이

clangd out-of-line 수정을 메모리에서 순차 합성 후 일괄 미리보기; 기존 대응 파일 필요, 200회·30초 한도; VA 동등성 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/SemanticRefactoringCommands.cs:43

```
            var deadline=DateTime.UtcNow.AddSeconds(30);
            Task<JToken> Query(string method,string text,int offset,JObject args=null,IDictionary<string,string> documents=null)
            {if(DateTime.UtcNow>deadline)throw new TimeoutException("Semantic refactoring exceeded 30 seconds; no files were changed.");var p=Position(text,offset);return CompilerSemanticService.Instance.QueryAsync(method,root,dte.FullName,file,text,(int)p["line"]+1,(int)p["character"]+1,token,args,documents??before);}
            if(command==0x1c0)
            {
                var input=EditorFeatureCommands.Prompt(dte,"New parameter order (one-based, e.g. 2,1; reorder only)","2,1");if(input==null)return;
                var order=input.Split(',').Select(v=>int.Parse(v.Trim(),System.Globalization.CultureInfo.InvariantCulture)-1).ToArray();
```

 CodeAssist.Extension/SemanticRefactoringCommands.cs:44

```
            Task<JToken> Query(string method,string text,int offset,JObject args=null,IDictionary<string,string> documents=null)
            {if(DateTime.UtcNow>deadline)throw new TimeoutException("Semantic refactoring exceeded 30 seconds; no files were changed.");var p=Position(text,offset);return CompilerSemanticService.Instance.QueryAsync(method,root,dte.FullName,file,text,(int)p["line"]+1,(int)p["character"]+1,token,args,documents??before);}
            if(command==0x1c0)
            {
                var input=EditorFeatureCommands.Prompt(dte,"New parameter order (one-based, e.g. 2,1; reorder only)","2,1");if(input==null)return;
                var order=input.Split(',').Select(v=>int.Parse(v.Trim(),System.Globalization.CultureInfo.InvariantCulture)-1).ToArray();
                var declarations=Locations(await Query("textDocument/declaration",active,caret)).Concat(Locations(await Query("textDocument/definition",active,caret))).ToArray();
```

 CodeAssist.Extension/SemanticRefactoringCommands.cs:64

```
            }
            var working=new Dictionary<string,string>(before,StringComparer.OrdinalIgnoreCase);var moved=0;
            try
            {
                for(var pass=0;pass<200;pass++)
                {
                    token.ThrowIfCancellationRequested();var current=working[file];JToken selectedEdit=null;
```

### 추가 실행 대조

- 중첩class·선택subset·template·Undo

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F024 · Move Selection to New File

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/refactoring/move-selection-to-new-file)

- 전체줄 선택만; destination은 미존재파일·header면 현재파일 include 옵션

- 언어·header/source별 생성snippet으로 새파일 내용 구성

### CA의 현재 범위·차이

선택 텍스트 새 파일 이동; include·프로젝트 추가 자동화 없음; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/EditorFeatureCommands.cs:55

```
            var view=components.GetService<IVsEditorAdaptersFactoryService>().GetWpfTextView(adapter);if(view==null||view.IsClosed)return;
            var snapshot=view.TextSnapshot;var selected=view.Selection.StreamSelectionSpan.SnapshotSpan;var caret=view.Caret.Position.BufferPosition.Position;var text=snapshot.GetText();
            var line=snapshot.GetLineFromPosition(caret);var word=IdentifierParser.At(line.GetText(),caret-line.Start.Position);
            if(id==0x17b)
            {
                var links=SourceLinks.Find(line.GetText(),StartupUi.Preferences.SourceLinkRules).Where(candidate=>caret-line.Start.Position>=candidate.Start&&caret-line.Start.Position<=candidate.Start+candidate.Length).ToArray();
                if(links.Length==0)throw new InvalidOperationException(UiText.Get("No source link matches the caret. Configure Navigation / Source Links.",StartupUi.Preferences.UiLanguage));
```

 CodeAssist.Extension/EditorFeatureCommands.cs:70

```
                var picker=new NavigationPicker("CodeAssist: Go To Member",entries,package.JoinableTaskFactory);new System.Windows.Interop.WindowInteropHelper(picker).Owner=(IntPtr)dte.MainWindow.HWnd;
                if(picker.ShowDialog()==true&&picker.Selected!=null){dte.ItemOperations.OpenFile(picker.Selected.File,EnvDTE.Constants.vsViewKindTextView);(dte.ActiveDocument.Selection as EnvDTE.TextSelection)?.MoveToLineAndOffset(picker.Selected.Line,picker.Selected.Column,false);}return;
            }
            if(id==0x175){if(selected.Length==0)return;SnippetInsertion.Show(view,file,indexes.ProjectRoot,(IntPtr)dte.MainWindow.HWnd,null,dte.Solution?.FullName,true);return;}
            if(id==0x176)
            {
                if(selected.Length==0)return;
```

 CodeAssist.Extension/EditorFeatureCommands.cs:76

```
                if(selected.Length==0)return;
                var title=Prompt(dte,"Snippet title","Selection");if(title==null)return;
                var shortcut=Prompt(dte,"Snippet shortcut","snippet");if(shortcut==null)return;
                var root=!string.IsNullOrEmpty(dte.Solution?.FullName)?Path.GetDirectoryName(dte.Solution.FullName):indexes.ProjectRoot;
                if(string.IsNullOrEmpty(root))throw new InvalidOperationException("Open a solution before saving a project snippet.");
                var path=Path.Combine(root,".codeassist","snippets.tpl");var existing=File.Exists(path)?File.ReadAllText(path):"";
                if(existing.Length>1024*1024)throw new InvalidOperationException("Snippet library exceeds 1 MiB.");
```

### 추가 실행 대조

- 부분줄·기존destination·언어template·include·Undo

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F025 · Rename

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/refactoring/rename)

- 선언/정의/참조 및 선택적 comment/string·상속chain 변경; project/solution 범위

- 이전 Rename/ChangeSignature/FindReferences scope 공유; 동일줄 복수ref는 복수항목

- scope변경으로 결과 재검색하면 제외선택 소실; preview double-click은 원본파일focus 유지

- Stop/Ctrl+Break로 검색중단 가능; 불완전검색 commit의 실패 가능성 문서 경고

### CA의 현재 범위·차이

clangd rename 작업 편집을 사용합니다. 실제 다중 파일 변경·Undo 및 VA 비교는 미검증입니다.

### 현재 코드 근거 후보

 CodeAssist.Extension/CompilerRename.cs:59

```
            }
            var result = await CompilerSemanticService.Instance.QueryAsync("textDocument/rename", root, dte.FullName, file, text, line, column,
                package.DisposalToken, new JObject { ["newName"] = name.Text }, open);
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (result == null) { MessageBox.Show("변경 계획을 만들지 못했습니다. " + CompilerSemanticService.Instance.LastError, "CodeAssist"); return true; }
            var plans = new List<Plan>();
            try
```

### 추가 실행 대조

- 상속·주석proximity·범위/제외·취소

- 다중project·중단검색·preview focus·Undo

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F026 · Rename Files

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/refactoring/rename-files)

- 관련 C++ h/cpp 및 C# designer/resx 함께 rename; include/project/solution/source-control 반영

- 기존directory 유지; rename전 저장·rename후 변경내용은 자동저장 안함

- 단일Undo 불가; nonproject/solution없는 파일에서는 의도 확인

### CA의 현재 범위·차이

프로젝트 항목 이름 변경; include 갱신 없음; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/EditorFeatureCommands.cs:141

```
            ThreadHelper.ThrowIfNotOnUIThread();
            var dialog=new Microsoft.Win32.SaveFileDialog {Title=UiText.Get("Create file",StartupUi.Preferences.UiLanguage),InitialDirectory=string.IsNullOrEmpty(source)?null:Path.GetDirectoryName(source),FileName="NewFile.cpp",Filter="C/C++ files|*.cpp;*.h;*.hpp;*.c|All files|*.*",OverwritePrompt=false};
            return dialog.ShowDialog()==true?dialog.FileName:null;
        }
        private static void WriteNew(string path,string contents)
        {using(var stream=new FileStream(path,FileMode.CreateNew,FileAccess.Write,FileShare.None))using(var writer=new StreamWriter(stream,new System.Text.UTF8Encoding(false)))writer.Write(contents);}
        internal static string Prompt(EnvDTE80.DTE2 dte,string label,string initial)
```

 CodeAssist.Extension/EditorFeatureCommands.cs:145

```
        private static void WriteNew(string path,string contents)
        {using(var stream=new FileStream(path,FileMode.CreateNew,FileAccess.Write,FileShare.None))using(var writer=new StreamWriter(stream,new System.Text.UTF8Encoding(false)))writer.Write(contents);}
        internal static string Prompt(EnvDTE80.DTE2 dte,string label,string initial)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var panel=new StackPanel {Margin=new Thickness(16)};ProductTheme.Apply(panel);panel.Children.Add(ProductTheme.Text(label));var input=new TextBox {Text=initial,Margin=new Thickness(0,0,0,16)};panel.Children.Add(input);
            var button=ProductTheme.Button("저장",()=>{},true);button.IsDefault=true;panel.Children.Add(button);
```

### 추가 실행 대조

- related files·include·SCC·충돌·read-only·save/Undo

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F027 · Simplify Instance Declaration

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/refactoring/simplify-instance-declaration)

- local object declaration에서만 제공; 생성된 instance를 불필요하게 다시 설정하는 declaration 단순화

### CA의 현재 범위·차이

동일 타입 생성자 재초기화 단순화; 지원 문법 제한; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/RefactoringCommands.cs:38

```
                case 0x18b:
                    result=CppDocumentTransforms.ConvertLocalPointer(snapshot.GetText(),start,length);start=0;length=snapshot.Length;title="Convert Local Pointer / Instance";break;
                case 0x18c:
                    if(selected.Length==0)throw new ArgumentException("Select statements to extract.");var signature=Ask("New free-function signature (use reference parameters for outputs)","void Extracted()");if(signature==null)return;
                    result=CppDocumentTransforms.ExtractGlobalMethod(snapshot.GetText(),start,length,signature);start=0;length=snapshot.Length;title="Extract Method";break;
                case 0x180:result=CppRefactoring.SimplifyInstance(source);title="Simplify Instance Declaration";break;
                case 0x181:
```

 CodeAssist.Extension/RefactoringCommands.cs:42

```
                    result=CppDocumentTransforms.ExtractGlobalMethod(snapshot.GetText(),start,length,signature);start=0;length=snapshot.Length;title="Extract Method";break;
                case 0x180:result=CppRefactoring.SimplifyInstance(source);title="Simplify Instance Declaration";break;
                case 0x181:
                    var property=Ask("Accessor name suffix","Value");if(property==null)return;result=CppRefactoring.EncapsulateField(source,property,nl);title="Encapsulate Field (declaration and accessors)";break;
                case 0x182:
                    var member=Ask("Member declaration","void NewMethod();");if(member==null)return;
                    if(!member.TrimEnd().EndsWith(";",StringComparison.Ordinal))throw new ArgumentException("A member declaration must end with a semicolon.");
```

 CodeAssist.Extension/RefactoringCommands.cs:44

```
                case 0x181:
                    var property=Ask("Accessor name suffix","Value");if(property==null)return;result=CppRefactoring.EncapsulateField(source,property,nl);title="Encapsulate Field (declaration and accessors)";break;
                case 0x182:
                    var member=Ask("Member declaration","void NewMethod();");if(member==null)return;
                    if(!member.TrimEnd().EndsWith(";",StringComparison.Ordinal))throw new ArgumentException("A member declaration must end with a semicolon.");
                    result=member+nl;start=line.Start;length=0;title="Add Member at Current Line";break;
                case 0x183:
```

### 추가 실행 대조

- local/nonlocal·타입/initializer·주석·Undo

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F028 · Add Forward Declaration

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-generation/add-forward-declaration)

- 미확정 참조 symbol에서 forward declaration 제공

- 기존 forward 선언 또는 첫 include 그룹 뒤 등 적절한 위치; 커서 유지

- 현재 symbol 참조를 근거로 하며 다른 참조를 평가해 include 충분성을 결정하지 않음

### CA의 현재 범위·차이

명시적으로 입력한 클래스·구조체 전방 선언; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/EditorFeatureCommands.cs:109

```
                var comment=CppGeneration.DocumentMethod(signature,text.Contains("\r\n")?"\r\n":"\n",indent);
                if(comment==null)throw new InvalidOperationException(UiText.Get("Select a method declaration or its signature line.",StartupUi.Preferences.UiLanguage));
                change=new EditorTextChange(signatureLine.Start,0,comment,signatureLine.Start.Position+comment.Length,0);
            }
            if(id==0x179)
            {
                var header=Prompt(dte,"Header name (use <...> for system headers)","");if(header==null)return;header=header.Trim();var system=header.StartsWith("<",StringComparison.Ordinal)&&header.EndsWith(">",StringComparison.Ordinal);
```

 CodeAssist.Extension/EditorFeatureCommands.cs:120

```
                var name=Prompt(dte,"Qualified type name (class or struct)",word);if(name==null)return;var structure=name.StartsWith("struct ",StringComparison.Ordinal);name=name.StartsWith("class ",StringComparison.Ordinal)?name.Substring(6):structure?name.Substring(7):name;
                var declaration=CppGeneration.ForwardDeclaration(name.Trim(),structure)+(text.Contains("\r\n")?"\r\n":"\n");change=new EditorTextChange(line.Start,0,declaration,line.Start.Position+declaration.Length,0);
            }
            if(change!=null)Apply(view,snapshot,change,components,"Code Generation");
        }
        internal static void Apply(IWpfTextView view,ITextSnapshot expected,EditorTextChange change,IComponentModel components,string name)
        {
```

### 추가 실행 대조

- pointer와 by-value 다른 사용 혼재

- namespace/template·include group·커서 유지

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F029 · Add Member

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-generation/add-member)

- class reference에서 signaturedialog 후method/variable 추가; C++method는 inline header

- CreateImplementation snippet 사용·system directory class에는 제공안함·VS현대버전 한Undo

### CA의 현재 범위·차이

현재 줄에 사용자 지정 멤버 선언 추가; 클래스 자동 탐색 없음; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/RefactoringCommands.cs:46

```
                case 0x182:
                    var member=Ask("Member declaration","void NewMethod();");if(member==null)return;
                    if(!member.TrimEnd().EndsWith(";",StringComparison.Ordinal))throw new ArgumentException("A member declaration must end with a semicolon.");
                    result=member+nl;start=line.Start;length=0;title="Add Member at Current Line";break;
                case 0x183:
                    var name=Ask("New member name","NewMember");if(name==null)return;var similar=CppRefactoring.SimilarMember(source,name);if(similar!=null)result=source+nl+similar;title="Add Similar Member";break;
                case 0x184:
```

 CodeAssist.Extension/RefactoringCommands.cs:47

```
                    var member=Ask("Member declaration","void NewMethod();");if(member==null)return;
                    if(!member.TrimEnd().EndsWith(";",StringComparison.Ordinal))throw new ArgumentException("A member declaration must end with a semicolon.");
                    result=member+nl;start=line.Start;length=0;title="Add Member at Current Line";break;
                case 0x183:
                    var name=Ask("New member name","NewMember");if(name==null)return;var similar=CppRefactoring.SimilarMember(source,name);if(similar!=null)result=source+nl+similar;title="Add Similar Member";break;
                case 0x184:
                    var ns=Ask("Namespace","std");if(ns==null)return;var directive=CppRefactoring.UsingNamespace(ns);if(directive!=null)result=directive+nl;start=line.Start;length=0;title="Add Using Directive at Current Line";break;
```

 CodeAssist.Extension/RefactoringCommands.cs:48

```
                    if(!member.TrimEnd().EndsWith(";",StringComparison.Ordinal))throw new ArgumentException("A member declaration must end with a semicolon.");
                    result=member+nl;start=line.Start;length=0;title="Add Member at Current Line";break;
                case 0x183:
                    var name=Ask("New member name","NewMember");if(name==null)return;var similar=CppRefactoring.SimilarMember(source,name);if(similar!=null)result=source+nl+similar;title="Add Similar Member";break;
                case 0x184:
                    var ns=Ask("Namespace","std");if(ns==null)return;var directive=CppRefactoring.UsingNamespace(ns);if(directive!=null)result=directive+nl;start=line.Start;length=0;title="Add Using Directive at Current Line";break;
                case 0x185:result=CppRefactoring.ToggleBraces(source);title="Add / Remove Braces";break;
```

### 추가 실행 대조

- reference·method/field·systemdir·snippet·Undo

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F030 · Add Missing Case Statements

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-generation/add-missing-case-statements)

- enum switch 안에서 누락된 enum case 추가; switch 키워드 자체 위치는 필수 아님

### CA의 현재 범위·차이

선택 switch와 사용자가 입력한 scoped enum에서 누락 case 추가; 복합 enum 표현식 제한; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/RefactoringCommands.cs:37

```
            {
                case 0x18b:
                    result=CppDocumentTransforms.ConvertLocalPointer(snapshot.GetText(),start,length);start=0;length=snapshot.Length;title="Convert Local Pointer / Instance";break;
                case 0x18c:
                    if(selected.Length==0)throw new ArgumentException("Select statements to extract.");var signature=Ask("New free-function signature (use reference parameters for outputs)","void Extracted()");if(signature==null)return;
                    result=CppDocumentTransforms.ExtractGlobalMethod(snapshot.GetText(),start,length,signature);start=0;length=snapshot.Length;title="Extract Method";break;
                case 0x180:result=CppRefactoring.SimplifyInstance(source);title="Simplify Instance Declaration";break;
```

 CodeAssist.Extension/RefactoringCommands.cs:39

```
                    result=CppDocumentTransforms.ConvertLocalPointer(snapshot.GetText(),start,length);start=0;length=snapshot.Length;title="Convert Local Pointer / Instance";break;
                case 0x18c:
                    if(selected.Length==0)throw new ArgumentException("Select statements to extract.");var signature=Ask("New free-function signature (use reference parameters for outputs)","void Extracted()");if(signature==null)return;
                    result=CppDocumentTransforms.ExtractGlobalMethod(snapshot.GetText(),start,length,signature);start=0;length=snapshot.Length;title="Extract Method";break;
                case 0x180:result=CppRefactoring.SimplifyInstance(source);title="Simplify Instance Declaration";break;
                case 0x181:
                    var property=Ask("Accessor name suffix","Value");if(property==null)return;result=CppRefactoring.EncapsulateField(source,property,nl);title="Encapsulate Field (declaration and accessors)";break;
```

 CodeAssist.Extension/RefactoringCommands.cs:40

```
                case 0x18c:
                    if(selected.Length==0)throw new ArgumentException("Select statements to extract.");var signature=Ask("New free-function signature (use reference parameters for outputs)","void Extracted()");if(signature==null)return;
                    result=CppDocumentTransforms.ExtractGlobalMethod(snapshot.GetText(),start,length,signature);start=0;length=snapshot.Length;title="Extract Method";break;
                case 0x180:result=CppRefactoring.SimplifyInstance(source);title="Simplify Instance Declaration";break;
                case 0x181:
                    var property=Ask("Accessor name suffix","Value");if(property==null)return;result=CppRefactoring.EncapsulateField(source,property,nl);title="Encapsulate Field (declaration and accessors)";break;
                case 0x182:
```

### 추가 실행 대조

- 일부/all enum 누락·default 존재

- 같은 값 alias·scoped enum·중첩 switch

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F031 · Add Similar Member

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-generation/add-similar-member)

- class아닌 memberreference에서 기존signature dialog·원member 근처에stub추가·body복사안함

- base member에서 실행하면 base class에 추가·systemdir 제외·현대VS 한Undo

### CA의 현재 범위·차이

선택 선언 복제 및 멤버명 교체; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/RefactoringCommands.cs:46

```
                case 0x182:
                    var member=Ask("Member declaration","void NewMethod();");if(member==null)return;
                    if(!member.TrimEnd().EndsWith(";",StringComparison.Ordinal))throw new ArgumentException("A member declaration must end with a semicolon.");
                    result=member+nl;start=line.Start;length=0;title="Add Member at Current Line";break;
                case 0x183:
                    var name=Ask("New member name","NewMember");if(name==null)return;var similar=CppRefactoring.SimilarMember(source,name);if(similar!=null)result=source+nl+similar;title="Add Similar Member";break;
                case 0x184:
```

 CodeAssist.Extension/RefactoringCommands.cs:47

```
                    var member=Ask("Member declaration","void NewMethod();");if(member==null)return;
                    if(!member.TrimEnd().EndsWith(";",StringComparison.Ordinal))throw new ArgumentException("A member declaration must end with a semicolon.");
                    result=member+nl;start=line.Start;length=0;title="Add Member at Current Line";break;
                case 0x183:
                    var name=Ask("New member name","NewMember");if(name==null)return;var similar=CppRefactoring.SimilarMember(source,name);if(similar!=null)result=source+nl+similar;title="Add Similar Member";break;
                case 0x184:
                    var ns=Ask("Namespace","std");if(ns==null)return;var directive=CppRefactoring.UsingNamespace(ns);if(directive!=null)result=directive+nl;start=line.Start;length=0;title="Add Using Directive at Current Line";break;
```

 CodeAssist.Extension/RefactoringCommands.cs:48

```
                    if(!member.TrimEnd().EndsWith(";",StringComparison.Ordinal))throw new ArgumentException("A member declaration must end with a semicolon.");
                    result=member+nl;start=line.Start;length=0;title="Add Member at Current Line";break;
                case 0x183:
                    var name=Ask("New member name","NewMember");if(name==null)return;var similar=CppRefactoring.SimilarMember(source,name);if(similar!=null)result=source+nl+similar;title="Add Similar Member";break;
                case 0x184:
                    var ns=Ask("Namespace","std");if(ns==null)return;var directive=CppRefactoring.UsingNamespace(ns);if(directive!=null)result=directive+nl;start=line.Start;length=0;title="Add Using Directive at Current Line";break;
                case 0x185:result=CppRefactoring.ToggleBraces(source);title="Add / Remove Braces";break;
```

### 추가 실행 대조

- base/derived·stub/body·배치·systemdir·Undo

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F032 · Add Using Directive

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-generation/add-using-directive)

- C# common.NET type 사용시목록suggestion·C#listbox활성필요

- accept하면상단using삽입·caret위치유지

### CA의 현재 범위·차이

현재 줄에 using namespace 지시문 추가; 참조 축약 없음; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/RefactoringCommands.cs:1

```
using CodeAssist.Core;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
```

 CodeAssist.Extension/RefactoringCommands.cs:2

```
using CodeAssist.Core;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
```

 CodeAssist.Extension/RefactoringCommands.cs:3

```
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
```

### 추가 실행 대조

- C#listbox설정·동명namespace·기존using·caret·Undo

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F033 · Add include Directive

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-generation/add-include-directive)

- 미확정 symbol의 파싱된 헤더 후보에서 include 생성

- 마지막 include 뒤의 ifdef 밖에 추가하고 커서 유지

- 정의 헤더 우선이며 Boost/Qt 일부는 umbrella 예외

- 기본 가장 짧은 경로; 상대/프로젝트 경로·quote/angle·filename-only 설정

- 기존 include delimiter를 따르며 없으면 IDE 설정 이용

### CA의 현재 범위·차이

중복 방지 include 추가; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/EditorFeatureCommands.cs:115

```
                var header=Prompt(dte,"Header name (use <...> for system headers)","");if(header==null)return;header=header.Trim();var system=header.StartsWith("<",StringComparison.Ordinal)&&header.EndsWith(">",StringComparison.Ordinal);
                change=CppGeneration.Include(text,system?header.Substring(1,header.Length-2):header,system);
            }
            if(id==0x17a)
            {
                var name=Prompt(dte,"Qualified type name (class or struct)",word);if(name==null)return;var structure=name.StartsWith("struct ",StringComparison.Ordinal);name=name.StartsWith("class ",StringComparison.Ordinal)?name.Substring(6):structure?name.Substring(7):name;
                var declaration=CppGeneration.ForwardDeclaration(name.Trim(),structure)+(text.Contains("\r\n")?"\r\n":"\n");change=new EditorTextChange(line.Start,0,declaration,line.Start.Position+declaration.Length,0);
```

### 추가 실행 대조

- ifdef 속 include·중복·umbrella·generated header

- 여러 include roots와 경로 스타일 옵션

- 설정별 후보 부재·커서 유지

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F034 · Add/Remove Braces

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-generation/addremove-braces)

- 단일statement add/remove/toggle·nested if/else/for/while 문맥

- 복수statement compound는remove불가·indent만으로복수줄add불가

- source상단일macro가복수statement로확장하면 remove가동작변경가능하다는문서경고

### CA의 현재 범위·차이

선택 문장 괄호 추가; 단일 함수 호출 문장만 제거 허용; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/RefactoringCommands.cs:53

```
                    var ns=Ask("Namespace","std");if(ns==null)return;var directive=CppRefactoring.UsingNamespace(ns);if(directive!=null)result=directive+nl;start=line.Start;length=0;title="Add Using Directive at Current Line";break;
                case 0x185:result=CppRefactoring.ToggleBraces(source);title="Add / Remove Braces";break;
                case 0x186:
                    var declaration=CppRefactoring.CreateDeclaration(source);if(declaration!=null)result=declaration+nl;
                    start=selected.Length==0?line.Start.Position:selected.Start.Position;length=0;title="Create Declaration before Selection";break;
                case 0x187:case 0x188:
                    var owner=Ask("Qualified owner type (empty for free function)",CppExpressionContext.FindEnclosingFunctionOwner(snapshot.GetText(),caret));if(owner==null)return;
```

 CodeAssist.Extension/RefactoringCommands.cs:70

```
            if(result==null)throw new InvalidOperationException(UiText.Get("The selected syntax is unsupported. Select the complete declaration or expression.",StartupUi.Preferences.UiLanguage));
            var after=snapshot.GetText().Remove(start,length).Insert(start,result);
            var tabs=new TabControl();tabs.Items.Add(new TabItem {Header=UiText.Get("Before",StartupUi.Preferences.UiLanguage),Content=Text(snapshot.GetText())});tabs.Items.Add(new TabItem {Header=UiText.Get("After",StartupUi.Preferences.UiLanguage),Content=Text(after)});tabs.SelectedIndex=1;
            var panel=new DockPanel();ProductTheme.Apply(panel);var apply=ProductTheme.Button("Apply changes",()=>{},true);DockPanel.SetDock(apply,Dock.Bottom);panel.Children.Add(apply);panel.Children.Add(tabs);
            var dialog=new Window {Title=title,Content=panel,Width=950,Height=650,WindowStartupLocation=WindowStartupLocation.CenterOwner};new System.Windows.Interop.WindowInteropHelper(dialog).Owner=(IntPtr)dte.MainWindow.HWnd;
            apply.Click+=(s,e)=>dialog.DialogResult=true;dialog.PreviewKeyDown+=(s,e)=>{if(e.Key==System.Windows.Input.Key.Escape)dialog.DialogResult=false;};
            if(dialog.ShowDialog()==true)EditorFeatureCommands.Apply(view,snapshot,new EditorTextChange(start,length,result,start,result.Length),model,title);
```

### 추가 실행 대조

- dangling else·nested·복수macro·toggle·Undo

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F035 · Create Declaration

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-generation/create-declaration)

- C/C++ 정의에서 일치 인수의 선언을 이웃 선언 근처 헤더에 생성

- 적절한 헤더 없으면 source에 생성

- 정의에서 주석이거나 없는 default 인수는 자동 노출하지 않음

### CA의 현재 범위·차이

선택 정의의 선언 생성·삽입; 헤더 자동 선택 없음; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/RefactoringCommands.cs:42

```
                    result=CppDocumentTransforms.ExtractGlobalMethod(snapshot.GetText(),start,length,signature);start=0;length=snapshot.Length;title="Extract Method";break;
                case 0x180:result=CppRefactoring.SimplifyInstance(source);title="Simplify Instance Declaration";break;
                case 0x181:
                    var property=Ask("Accessor name suffix","Value");if(property==null)return;result=CppRefactoring.EncapsulateField(source,property,nl);title="Encapsulate Field (declaration and accessors)";break;
                case 0x182:
                    var member=Ask("Member declaration","void NewMethod();");if(member==null)return;
                    if(!member.TrimEnd().EndsWith(";",StringComparison.Ordinal))throw new ArgumentException("A member declaration must end with a semicolon.");
```

 CodeAssist.Extension/RefactoringCommands.cs:44

```
                case 0x181:
                    var property=Ask("Accessor name suffix","Value");if(property==null)return;result=CppRefactoring.EncapsulateField(source,property,nl);title="Encapsulate Field (declaration and accessors)";break;
                case 0x182:
                    var member=Ask("Member declaration","void NewMethod();");if(member==null)return;
                    if(!member.TrimEnd().EndsWith(";",StringComparison.Ordinal))throw new ArgumentException("A member declaration must end with a semicolon.");
                    result=member+nl;start=line.Start;length=0;title="Add Member at Current Line";break;
                case 0x183:
```

 CodeAssist.Extension/RefactoringCommands.cs:46

```
                case 0x182:
                    var member=Ask("Member declaration","void NewMethod();");if(member==null)return;
                    if(!member.TrimEnd().EndsWith(";",StringComparison.Ordinal))throw new ArgumentException("A member declaration must end with a semicolon.");
                    result=member+nl;start=line.Start;length=0;title="Add Member at Current Line";break;
                case 0x183:
                    var name=Ask("New member name","NewMember");if(name==null)return;var similar=CppRefactoring.SimilarMember(source,name);if(similar!=null)result=source+nl+similar;title="Add Similar Member";break;
                case 0x184:
```

### 추가 실행 대조

- 헤더 존재/없음·이웃 순서

- 기본 인수 주석·static/const/template

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F036 · Create File

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-generation/create-file)

- 현재파일 directory에 생성·현재project에추가·C++/C#/idl/rc/xaml관련파일에서제공

- 짝없는 h/cpp는 대응파일이 기본후보·cpp생성시同名headerinclude·header면 include옵션

- CreateFile snippet이 초기내용/caret 위치 결정

### CA의 현재 범위·차이

새 파일 생성·열기; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/EditorFeatureCommands.cs:83

```
                if(SnippetTemplate.Parse(existing).Any(t=>t.Shortcut==shortcut))throw new InvalidOperationException("Snippet shortcut already exists.");
                var entry=CppGeneration.SnippetFromSelection(title,shortcut,selected.GetText());Directory.CreateDirectory(Path.GetDirectoryName(path));
                var temporary=path+"."+Guid.NewGuid().ToString("N")+".tmp";
                try{File.WriteAllText(temporary,existing+(existing.Length==0?"":"\f\n")+entry);if(File.Exists(path))File.Replace(temporary,path,path+".bak");else File.Move(temporary,path);}finally{if(File.Exists(temporary))File.Delete(temporary);}return;
            }
            if(id==0x177)
            {
```

 CodeAssist.Extension/EditorFeatureCommands.cs:129

```
            components.GetService<ITextUndoHistoryRegistry>().TryGetHistory(view.TextBuffer,out var history);var operations=components.GetService<IEditorOperationsFactoryService>().GetEditorOperations(view);
            using(var transaction=history?.CreateTransaction("CodeAssist "+name))using(var edit=view.TextBuffer.CreateEdit())
            {
                if(transaction!=null)operations.AddBeforeTextBufferChangePrimitive();
                if(!edit.Replace(change.Start,change.Length,change.Text))throw new InvalidOperationException("The selected range is read-only.");
                var updated=edit.Apply();if(edit.Canceled)throw new InvalidOperationException("Editor rejected the edit.");
                view.Selection.Select(new SnapshotSpan(updated,change.SelectionStart,change.SelectionLength),false);view.Caret.MoveTo(new SnapshotPoint(updated,change.SelectionStart+change.SelectionLength));
```

 CodeAssist.Extension/EditorFeatureCommands.cs:141

```
            ThreadHelper.ThrowIfNotOnUIThread();
            var dialog=new Microsoft.Win32.SaveFileDialog {Title=UiText.Get("Create file",StartupUi.Preferences.UiLanguage),InitialDirectory=string.IsNullOrEmpty(source)?null:Path.GetDirectoryName(source),FileName="NewFile.cpp",Filter="C/C++ files|*.cpp;*.h;*.hpp;*.c|All files|*.*",OverwritePrompt=false};
            return dialog.ShowDialog()==true?dialog.FileName:null;
        }
        private static void WriteNew(string path,string contents)
        {using(var stream=new FileStream(path,FileMode.CreateNew,FileAccess.Write,FileShare.None))using(var writer=new StreamWriter(stream,new System.Text.UTF8Encoding(false)))writer.Write(contents);}
        internal static string Prompt(EnvDTE80.DTE2 dte,string label,string initial)
```

### 추가 실행 대조

- 짝없는파일·언어별·project외·include·caret

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F037 · Create from Usage

실행 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-generation/create-from-usage)

- class·enum·변수·함수 및 local/class/global 생성 선택

- 클래스 생성은 생성자 인수에서 멤버/초기화 추론; 파일명 변경 가능

- 미확정 타입은 UnknownType; 생성 문법/본문은 언어별 스니펫

- 인수 생성은 선언/정의에 반영; enum 사용 문맥 및 scope 구분

### CA의 현재 범위·차이

기존 장부에 구체적인 구현 범위 설명 누락. 후보 파일 존재만 확인함.

### 현재 코드 근거 후보

 CodeAssist.Extension/NavigationCommands.cs:81

```
                            if (dialog.ShowDialog() != true || view.IsClosed || !ReferenceEquals(snapshot, view.TextSnapshot)) return;
                            var change = SelectedLineSort.Create(snapshot.GetText(), selected.Start.Position, selected.Length, descending.IsChecked == true, ignoreCase.IsChecked == true);
                            using (var edit = view.TextBuffer.CreateEdit())
                            {
                                if (!edit.Replace(new Span(change.Start, change.Length), change.Text)) return;
                                var updated = edit.Apply();
                                view.Selection.Select(new SnapshotSpan(updated, change.Start, change.Text.Length), false);
```

 CodeAssist.Extension/NavigationCommands.cs:82

```
                            var change = SelectedLineSort.Create(snapshot.GetText(), selected.Start.Position, selected.Length, descending.IsChecked == true, ignoreCase.IsChecked == true);
                            using (var edit = view.TextBuffer.CreateEdit())
                            {
                                if (!edit.Replace(new Span(change.Start, change.Length), change.Text)) return;
                                var updated = edit.Apply();
                                view.Selection.Select(new SnapshotSpan(updated, change.Start, change.Text.Length), false);
                                view.Caret.MoveTo(new SnapshotPoint(updated, change.Start + change.Text.Length));
```

 CodeAssist.Extension/NavigationCommands.cs:97

```
                            {
                                tracker = view.TextBuffer.Properties.GetOrCreateSingletonProperty(() =>
                                    new LiveDocumentTracker(view.TextBuffer, document.FilePath, indexes));
                                tracker.Attach(view);
                            }
                        }
                        else if (command == 0x010f) view.ZoomLevel = 100;
```

### 추가 실행 대조

- new Foo와 new Foo()·상수 생성자 인수

- 클래스 멤버/지역/전역/인수/enum 선택

- 추론 실패 UnknownType 및 snippet customizations

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F038 · Create Implementation

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-generation/create-implementation)

- C/C++ 메서드/정적 멤버 선언에서 stub 생성

- 기본 인수는 기본적으로 block comment로 구현 인수에 유지; 옵션으로 생략

- 이웃 구현 근처/EOF 위치 설정; source 없으면 헤더에 생성

- 언어별 스니펫으로 서식; 필수 snippet 삭제 시 기본 복원

### CA의 현재 범위·차이

선택 선언에서 파일 끝에 구현 틀 생성; throw 미구현 본문; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/RefactoringCommands.cs:55

```
                case 0x186:
                    var declaration=CppRefactoring.CreateDeclaration(source);if(declaration!=null)result=declaration+nl;
                    start=selected.Length==0?line.Start.Position:selected.Start.Position;length=0;title="Create Declaration before Selection";break;
                case 0x187:case 0x188:
                    var owner=Ask("Qualified owner type (empty for free function)",CppExpressionContext.FindEnclosingFunctionOwner(snapshot.GetText(),caret));if(owner==null)return;
                    result=id==0x187?CppRefactoring.CreateImplementation(source,owner,nl,false,StartupUi.Preferences.MethodBodyTemplate):CppRefactoring.CreateImplementations(source,owner,nl,false,StartupUi.Preferences.MethodBodyTemplate);
                    if(result!=null)result=nl+result;start=snapshot.Length;length=0;title="Generate Method Implementation(s) at End of File";break;
```

 CodeAssist.Extension/RefactoringCommands.cs:56

```
                    var declaration=CppRefactoring.CreateDeclaration(source);if(declaration!=null)result=declaration+nl;
                    start=selected.Length==0?line.Start.Position:selected.Start.Position;length=0;title="Create Declaration before Selection";break;
                case 0x187:case 0x188:
                    var owner=Ask("Qualified owner type (empty for free function)",CppExpressionContext.FindEnclosingFunctionOwner(snapshot.GetText(),caret));if(owner==null)return;
                    result=id==0x187?CppRefactoring.CreateImplementation(source,owner,nl,false,StartupUi.Preferences.MethodBodyTemplate):CppRefactoring.CreateImplementations(source,owner,nl,false,StartupUi.Preferences.MethodBodyTemplate);
                    if(result!=null)result=nl+result;start=snapshot.Length;length=0;title="Generate Method Implementation(s) at End of File";break;
                case 0x189:
```

 CodeAssist.Extension/RefactoringCommands.cs:59

```
                    var owner=Ask("Qualified owner type (empty for free function)",CppExpressionContext.FindEnclosingFunctionOwner(snapshot.GetText(),caret));if(owner==null)return;
                    result=id==0x187?CppRefactoring.CreateImplementation(source,owner,nl,false,StartupUi.Preferences.MethodBodyTemplate):CppRefactoring.CreateImplementations(source,owner,nl,false,StartupUi.Preferences.MethodBodyTemplate);
                    if(result!=null)result=nl+result;start=snapshot.Length;length=0;title="Generate Method Implementation(s) at End of File";break;
                case 0x189:
                    var virtuals=Ask("Virtual declarations to implement in the derived class","virtual void Update() = 0;");if(virtuals==null)return;
                    result=CppRefactoring.CreateImplementations(virtuals,"",nl,true,StartupUi.Preferences.MethodBodyTemplate);start=caret;length=0;title="Implement Virtual Methods at Caret";break;
                case 0x18a:
```

### 추가 실행 대조

- source 존재/없음·EOF/이웃 배치

- default 인수 옵션·정적 멤버·snippet 삭제

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F039 · Create Method Implementations

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-generation/create-method-implementations)

- class declaration에서미구현method선택·구현된method는목록제외

- 대응source없으면 declaration아래생성·생성snippet·인접/EOF 옵션

### CA의 현재 범위·차이

선택한 여러 선언의 구현 틀 생성; 지원 문법 제한; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/RefactoringCommands.cs:41

```
                    if(selected.Length==0)throw new ArgumentException("Select statements to extract.");var signature=Ask("New free-function signature (use reference parameters for outputs)","void Extracted()");if(signature==null)return;
                    result=CppDocumentTransforms.ExtractGlobalMethod(snapshot.GetText(),start,length,signature);start=0;length=snapshot.Length;title="Extract Method";break;
                case 0x180:result=CppRefactoring.SimplifyInstance(source);title="Simplify Instance Declaration";break;
                case 0x181:
                    var property=Ask("Accessor name suffix","Value");if(property==null)return;result=CppRefactoring.EncapsulateField(source,property,nl);title="Encapsulate Field (declaration and accessors)";break;
                case 0x182:
                    var member=Ask("Member declaration","void NewMethod();");if(member==null)return;
```

 CodeAssist.Extension/RefactoringCommands.cs:46

```
                case 0x182:
                    var member=Ask("Member declaration","void NewMethod();");if(member==null)return;
                    if(!member.TrimEnd().EndsWith(";",StringComparison.Ordinal))throw new ArgumentException("A member declaration must end with a semicolon.");
                    result=member+nl;start=line.Start;length=0;title="Add Member at Current Line";break;
                case 0x183:
                    var name=Ask("New member name","NewMember");if(name==null)return;var similar=CppRefactoring.SimilarMember(source,name);if(similar!=null)result=source+nl+similar;title="Add Similar Member";break;
                case 0x184:
```

 CodeAssist.Extension/RefactoringCommands.cs:55

```
                case 0x186:
                    var declaration=CppRefactoring.CreateDeclaration(source);if(declaration!=null)result=declaration+nl;
                    start=selected.Length==0?line.Start.Position:selected.Start.Position;length=0;title="Create Declaration before Selection";break;
                case 0x187:case 0x188:
                    var owner=Ask("Qualified owner type (empty for free function)",CppExpressionContext.FindEnclosingFunctionOwner(snapshot.GetText(),caret));if(owner==null)return;
                    result=id==0x187?CppRefactoring.CreateImplementation(source,owner,nl,false,StartupUi.Preferences.MethodBodyTemplate):CppRefactoring.CreateImplementations(source,owner,nl,false,StartupUi.Preferences.MethodBodyTemplate);
                    if(result!=null)result=nl+result;start=snapshot.Length;length=0;title="Generate Method Implementation(s) at End of File";break;
```

### 추가 실행 대조

- 일부기구현·대응없음·overload·배치·Undo

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F040 · Document Method

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-generation/document-method)

- 선언/정의에서 method 앞 commentblock·return/parameter 각각의 문서항목 생성

- DocumentMethod snippet과 refactoring전용 reserved값으로형식변경

### CA의 현재 범위·차이

메서드 Doxygen 주석 생성; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/EditorFeatureCommands.cs:40

```
            var dte=await package.GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;if(dte==null)return;
            var file=dte.ActiveDocument?.FullName;
            if(id==0x172){var path=ChooseNewFile(file);if(path==null)return;WriteNew(path,"");dte.ItemOperations.OpenFile(path,EnvDTE.Constants.vsViewKindTextView);return;}
            if(id==0x173)
            {
                var document=dte.ActiveDocument;var item=document?.ProjectItem;
                if(document==null||item==null)throw new InvalidOperationException(UiText.Get("Select a project file first.",StartupUi.Preferences.UiLanguage));
```

 CodeAssist.Extension/EditorFeatureCommands.cs:44

```
            {
                var document=dte.ActiveDocument;var item=document?.ProjectItem;
                if(document==null||item==null)throw new InvalidOperationException(UiText.Get("Select a project file first.",StartupUi.Preferences.UiLanguage));
                if(!document.Saved)throw new InvalidOperationException(UiText.Get("Save the document before renaming it.",StartupUi.Preferences.UiLanguage));
                var name=Prompt(dte,"New file name",Path.GetFileName(file));if(name==null)return;
                if(string.IsNullOrWhiteSpace(name)||Path.GetFileName(name)!=name||name.IndexOfAny(Path.GetInvalidFileNameChars())>=0)throw new ArgumentException("Invalid file name");
                item.Name=name;return;
```

 CodeAssist.Extension/EditorFeatureCommands.cs:45

```
                var document=dte.ActiveDocument;var item=document?.ProjectItem;
                if(document==null||item==null)throw new InvalidOperationException(UiText.Get("Select a project file first.",StartupUi.Preferences.UiLanguage));
                if(!document.Saved)throw new InvalidOperationException(UiText.Get("Save the document before renaming it.",StartupUi.Preferences.UiLanguage));
                var name=Prompt(dte,"New file name",Path.GetFileName(file));if(name==null)return;
                if(string.IsNullOrWhiteSpace(name)||Path.GetFileName(name)!=name||name.IndexOfAny(Path.GetInvalidFileNameChars())>=0)throw new ArgumentException("Invalid file name");
                item.Name=name;return;
            }
```

### 추가 실행 대조

- declaration/definition·return/parameter·snippet·Undo

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F041 · Implement Interface / Virtual Methods

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-generation/implement-interface-virtual-methods)

- 깊은 상속과 다중 상속의 메서드 후보; 구현된 것은 선택 불가

- pure virtual은 기본 체크, 나머지는 기본 미체크

- 필터를 바꿔도 체크 상태 유지; 기본 정렬은 선언 순서

- virtual/override 생성 키워드와 본문 snippet 설정

### CA의 현재 범위·차이

사용자가 입력한 가상 선언의 override 구현 틀 삽입; 기저 클래스 자동 열거 없음; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/RefactoringCommands.cs:59

```
                    var owner=Ask("Qualified owner type (empty for free function)",CppExpressionContext.FindEnclosingFunctionOwner(snapshot.GetText(),caret));if(owner==null)return;
                    result=id==0x187?CppRefactoring.CreateImplementation(source,owner,nl,false,StartupUi.Preferences.MethodBodyTemplate):CppRefactoring.CreateImplementations(source,owner,nl,false,StartupUi.Preferences.MethodBodyTemplate);
                    if(result!=null)result=nl+result;start=snapshot.Length;length=0;title="Generate Method Implementation(s) at End of File";break;
                case 0x189:
                    var virtuals=Ask("Virtual declarations to implement in the derived class","virtual void Update() = 0;");if(virtuals==null)return;
                    result=CppRefactoring.CreateImplementations(virtuals,"",nl,true,StartupUi.Preferences.MethodBodyTemplate);start=caret;length=0;title="Implement Virtual Methods at Caret";break;
                case 0x18a:
```

 CodeAssist.Extension/RefactoringCommands.cs:60

```
                    result=id==0x187?CppRefactoring.CreateImplementation(source,owner,nl,false,StartupUi.Preferences.MethodBodyTemplate):CppRefactoring.CreateImplementations(source,owner,nl,false,StartupUi.Preferences.MethodBodyTemplate);
                    if(result!=null)result=nl+result;start=snapshot.Length;length=0;title="Generate Method Implementation(s) at End of File";break;
                case 0x189:
                    var virtuals=Ask("Virtual declarations to implement in the derived class","virtual void Update() = 0;");if(virtuals==null)return;
                    result=CppRefactoring.CreateImplementations(virtuals,"",nl,true,StartupUi.Preferences.MethodBodyTemplate);start=caret;length=0;title="Implement Virtual Methods at Caret";break;
                case 0x18a:
                    var enumName=Ask("Qualified scoped enum name","State");if(enumName==null)return;
```

 CodeAssist.Extension/RefactoringCommands.cs:62

```
                case 0x189:
                    var virtuals=Ask("Virtual declarations to implement in the derived class","virtual void Update() = 0;");if(virtuals==null)return;
                    result=CppRefactoring.CreateImplementations(virtuals,"",nl,true,StartupUi.Preferences.MethodBodyTemplate);start=caret;length=0;title="Implement Virtual Methods at Caret";break;
                case 0x18a:
                    var enumName=Ask("Qualified scoped enum name","State");if(enumName==null)return;
                    var definition=Ask("Enum declaration","enum class State { Ready, Running, Done };");if(definition==null)return;
                    result=CppRefactoring.MissingCases(source,definition,enumName,nl);title="Add Missing Case Statements";break;
```

### 추가 실행 대조

- 다중/diamond 상속·이미 구현한 메서드

- 필터 반복 후 체크 유지·정렬 전환

- 추상/구체 혼합과 virtual/override 설정

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F042 · Acronyms

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/coding-assistance/acronyms)

- 축소 목록 옵션이 필요하며 첫 글자와 대문자/underscore 경계로 후보를 좁힘

- project 및 최근 사용 symbol 중심; 파일/심볼 검색 dialog와 method list에서는 사용하지 않음

### CA의 현재 범위·차이

머리글자 약어 추천 후보; IDE 필터 통과 미검증; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CodeAssistCompletionSource.cs:35

```
        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            if (DocumentFactory.TryGetTextDocument(textBuffer, out var document))
            {
                IndexService.InitializeFromDocument(document.FilePath);
            }
```

### 추가 실행 대조

- camel/Pascal/underscore·중복 약어·최근 사용·목록별 가용성

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F043 · Auto-Extend Multi-Line Comments

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- 줄바꿈 시 block comment 및 연속 single-line comment marker를 현재 줄에서 이어받음

### CA의 현재 범위·차이

주석 이어쓰기; 원본 비교 미완료

### 현재 코드 근거 후보

 CodeAssist.Core/CppEditing.cs:19

```
            var value=text.Substring(start,length);
            if(key=='/'&&(value.IndexOf('\n')>=0||WholeLine(text,start,length)))return ToggleLines(text,start,length,spaceAfterSlash);
            if(key=='/'||key=='*')
            {
                if(value.StartsWith("/*",StringComparison.Ordinal)&&value.EndsWith("*/",StringComparison.Ordinal)&&value.Length>=4)
                {var inner=value.Substring(2,value.Length-4);return new EditorTextChange(start,length,inner,start,inner.Length);}
                if(value.Contains("*/"))return null;
```

 CodeAssist.Core/CppEditing.cs:30

```
        }
        private static bool WholeLine(string text,int start,int length)
        {
            var left=start==0?0:text.LastIndexOf('\n',start-1)+1;
            var right=text.IndexOf('\n',start+length);
            if(right<0)right=text.Length;
            return string.IsNullOrWhiteSpace(text.Substring(left,start-left))&&string.IsNullOrWhiteSpace(text.Substring(start+length,right-start-length));
```

 CodeAssist.Core/CppEditing.cs:37

```
        }
        private static EditorTextChange ToggleLines(string text,int start,int length,bool space)
        {
            var left=start==0?0:text.LastIndexOf('\n',start-1)+1;
            var selectedEnd=start+length;
            var right=selectedEnd>0&&text[selectedEnd-1]=='\n'?selectedEnd:text.IndexOf('\n',selectedEnd);
            if(right<0)right=text.Length;
```

 CodeAssist.Extension/CompletionCommandFilter.cs:112

```
            if(EditorInputTrace.Enabled && tracedInputs++<24)WelcomeDocumentWindow.RecordEvent("EditorInput group="+commandGroup+" id="+commandId+" typed="+(hasTypedCharacter?(int)typedCharacter:-1)+" selection="+view.Selection.StreamSelectionSpan.SnapshotSpan.Length+" surround="+StartupUi.Preferences.SurroundSelectionEnabled);
            if(hasTypedCharacter&&view.Selection.IsEmpty&&TryAutomaticInput(typedCharacter))return VSConstants.S_OK;
            if (hasTypedCharacter && TrySurroundSelection(typedCharacter)) return VSConstants.S_OK;
            if(hasTypedCharacter && TrySkipClosing(typedCharacter))return VSConstants.S_OK;
            if(hasTypedCharacter && StartupUi.Preferences.PairCompletionEnabled && CppPairing.Opening(typedCharacter))
            {
                var selected=view.Selection.StreamSelectionSpan.SnapshotSpan;
```

 CodeAssist.Extension/CompletionCommandFilter.cs:123

```
            if (commandGroup==VSConstants.VSStd2K && commandId==(uint)VSConstants.VSStd2KCmdID.RETURN &&
                StartupUi.Preferences.ExtendCommentsEnabled && view.Selection.IsEmpty && !broker.IsCompletionActive(view))
            {
                var prefix=CppEditing.CommentContinuation(view.TextSnapshot.GetText(),view.Caret.Position.BufferPosition.Position);
                if(prefix!=null)return ContinueComment(ref commandGroup,commandId,commandOptions,input,output,prefix);
            }
            var pasteSpan=StartupUi.Preferences.FormatAfterPasteEnabled && commandGroup==VSConstants.GUID_VSStandardCommandSet97 && commandId==(uint)VSConstants.VSStd97CmdID.Paste && view.Selection.SelectedSpans.Count==1
```

 CodeAssist.Extension/CompletionCommandFilter.cs:162

```
            var result = Next.Exec(ref commandGroup, commandId, commandOptions, input, output);
            if(ErrorHandler.Succeeded(result) && pasteSpan!=null) FormatPastedLines(pasteSpan);
            if (ErrorHandler.Succeeded(result) && copiedText != null) EditorClipboardHistory.History.Record(copiedText);
            if (ErrorHandler.Succeeded(result) && (hasTypedCharacter || isDeletion))
            {
                RefreshCodeAssistSessions();
            }
```

### 추가 실행 대조

- block·single-line·끝줄·선두공백 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F044 · Enhanced Listboxes

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/coding-assistance/enhanced-listboxes)

- member는 dot/arrow, completion은 Ctrl+Space; private member도 목록 표시

- C/C++ 기본 활성·C# 기본 비활성이라는 문서 조건; 언어별 원본 값 별도 검증 필요

- 비상속 bold/우선 배치·축소·acronym/shorthand·종류/가시성 toolbar 필터

- Tab/Enter 비활성은 목록을 닫고 입력 문자 삽입; Ctrl 투명화; 추가 commit 문자는 IDE/언어별 차이

- include 경로·snippet 및 주석/문자열 Ctrl+Space 문맥 없는 파일 텍스트 후보

- C/C++ 데이터 공급원 선택; :: 및 C#은 default IntelliSense; 우측 텍스트 overwrite는 별도 valid symbol 여부 영향

### CA의 현재 범위·차이

추천 색상·자체 아이콘·VS 기본색 복원; 원본 필터/UI 전체 미완료

### 현재 코드 근거 후보

 CodeAssist.Extension/CompletionColorProvider.cs:19

```
        internal SymbolKind Kind { get; }
        internal ColoredCompletion(SymbolKind kind,string display,string insert,string description,ImageMoniker icon,string automation,IEnumerable<CompletionIcon2> attributes,string suffix)
            : base(display,insert,description,icon,automation,attributes,suffix) { Kind=kind; }
    }
    [Export(typeof(IUIElementProvider<Completion,ICompletionSession>))]
    [Name("CodeAssist Completion Colors")]
    [ContentType("C/C++")]
```

 CodeAssist.Extension/CodeAssistCompletionSource.cs:35

```
        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            if (DocumentFactory.TryGetTextDocument(textBuffer, out var document))
            {
                IndexService.InitializeFromDocument(document.FilePath);
            }
```

### 추가 실행 대조

- member/completion/suggestion 전환 및 필터 조합 비교

- commit 설정·우측 텍스트 overwrite·취소 비교

- C++/C#·::·include·주석 데이터 공급원 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F045 · Format After Paste

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- 여러 줄 paste 이후 IDE Format Selection으로 서식 지정

- Undo로 formatting 철회 가능

### CA의 현재 범위·차이

다중 줄 붙여넣기 후 VS 선택 서식 명령; 별도 Undo 가능

### 현재 코드 근거 후보

 CodeAssist.Extension/CompletionCommandFilter.cs:128

```
            }
            var pasteSpan=StartupUi.Preferences.FormatAfterPasteEnabled && commandGroup==VSConstants.GUID_VSStandardCommandSet97 && commandId==(uint)VSConstants.VSStd97CmdID.Paste && view.Selection.SelectedSpans.Count==1
                ?view.Selection.StreamSelectionSpan.SnapshotSpan.Snapshot.CreateTrackingSpan(view.Selection.StreamSelectionSpan.SnapshotSpan.Span,SpanTrackingMode.EdgeInclusive):null;
            var isDeletion = IsDeletion(commandGroup, commandId);
            var isExplicitCompletion = IsExplicitCompletion(commandGroup, commandId);
            // Handle our explicit list before VC++ can expand the word on its own.
            // Otherwise COMPLETEWORD edits the buffer, then our fallback opens a
```

 CodeAssist.Extension/CompletionCommandFilter.cs:162

```
            var result = Next.Exec(ref commandGroup, commandId, commandOptions, input, output);
            if(ErrorHandler.Succeeded(result) && pasteSpan!=null) FormatPastedLines(pasteSpan);
            if (ErrorHandler.Succeeded(result) && copiedText != null) EditorClipboardHistory.History.Record(copiedText);
            if (ErrorHandler.Succeeded(result) && (hasTypedCharacter || isDeletion))
            {
                RefreshCodeAssistSessions();
            }
```

 CodeAssist.Extension/CompletionCommandFilter.cs:370

```
        }
        private void FormatPastedLines(ITrackingSpan inserted)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if(view.IsClosed)return;
            var snapshot=view.TextSnapshot;var span=inserted.GetSpan(snapshot);
            if(span.Length==0||span.GetText().IndexOf('\n')<0)return;
```

### 추가 실행 대조

- 단일/다중 줄 paste 및 Undo 단계 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F046 · Insert () and Closing Brackets and Quotes

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- 여는 delimiter 뒤 닫는 delimiter·method commit 후 괄호 삽입

- Undo/Backspace 철회; 입력 중단 전 inserted delimiter 입력은 해당 문자 덮어쓰기

- 닫는 brace 위치는 same/next/blank-line 옵션

### CA의 현재 범위·차이

괄호·따옴표·중괄호 위치·함수 괄호; 원본 비교 미완료

### 현재 코드 근거 후보

 CodeAssist.Core/CppPairing.cs:5

```
{
    public enum ClosingBracePlacement { BlankLine, NextLine, SameLine }
    public sealed class PairInsertion
    {
        public PairInsertion(string text,int caretOffset,int closingOffset){Text=text;CaretOffset=caretOffset;ClosingOffset=closingOffset;}
        public string Text{get;} public int CaretOffset{get;} public int ClosingOffset{get;}
    }
```

 CodeAssist.Core/CppPairing.cs:6

```
    public enum ClosingBracePlacement { BlankLine, NextLine, SameLine }
    public sealed class PairInsertion
    {
        public PairInsertion(string text,int caretOffset,int closingOffset){Text=text;CaretOffset=caretOffset;ClosingOffset=closingOffset;}
        public string Text{get;} public int CaretOffset{get;} public int ClosingOffset{get;}
    }
    public static class CppPairing
```

 CodeAssist.Core/CppPairing.cs:8

```
    {
        public PairInsertion(string text,int caretOffset,int closingOffset){Text=text;CaretOffset=caretOffset;ClosingOffset=closingOffset;}
        public string Text{get;} public int CaretOffset{get;} public int ClosingOffset{get;}
    }
    public static class CppPairing
    {
        public static bool Opening(char key)=>key=='('||key=='['||key=='{'||key=='\''||key=='"';
```

 CodeAssist.Extension/CompletionCommandFilter.cs:60

```
        private bool liveCompletionQueued;
        private sealed class OwnedPair { internal ITrackingPoint Open,Close;internal char Opening,Closing; }
        private readonly List<OwnedPair> pairs=new List<OwnedPair>();
        private int tracedInputs;

        public CompletionCommandFilter(IWpfTextView view, ICompletionBroker broker, ITextUndoHistoryRegistry undoHistories, IEditorOperations editorOperations, ProjectIndexService indexes=null)
        {
```

 CodeAssist.Extension/CompletionCommandFilter.cs:104

```
                copiedText = view.Selection.SelectedSpans[0].GetText();
            if(commandGroup==VSConstants.VSStd2K&&commandId==(uint)VSConstants.VSStd2KCmdID.TAB&&!broker.IsCompletionActive(view)&&SnippetInsertion.TryShortcut(view))return VSConstants.S_OK;
            if (IsCommitCommand(commandGroup, commandId) && TryCommitCodeAssistSelection())
            {
                return VSConstants.S_OK;
            }
```

 CodeAssist.Extension/CompletionCommandFilter.cs:114

```
            if (hasTypedCharacter && TrySurroundSelection(typedCharacter)) return VSConstants.S_OK;
            if(hasTypedCharacter && TrySkipClosing(typedCharacter))return VSConstants.S_OK;
            if(hasTypedCharacter && StartupUi.Preferences.PairCompletionEnabled && CppPairing.Opening(typedCharacter))
            {
                var selected=view.Selection.StreamSelectionSpan.SnapshotSpan;
                var text=view.TextSnapshot.GetText();
                if(text.Length<=2*1024*1024&&CppPairing.CanOpen(text.Remove(selected.Start,selected.Length),selected.Start,typedCharacter))
```

### 추가 실행 대조

- typing 중단·중복닫힘·commit·brace 위치 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F047 · Insert _ After m and Shift

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- m 입력 직후 Shift를 빠르게 눌렀다 놓으면 underscore 삽입

### CA의 현재 범위·차이

m 다음 짧은 Shift 입력으로 밑줄 추가; 옵션 기본 꺼짐; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CompletionCommandFilter.cs:53

```
        private readonly ProjectIndexService indexes;
        private long shiftStarted;private int shiftCaret=-1;
        private readonly IWpfTextView view;
        private readonly ICompletionBroker broker;
        private readonly ITextUndoHistoryRegistry undoHistories;
        private readonly IEditorOperations editorOperations;
        private readonly SynchronizationContext uiContext;
```

 CodeAssist.Extension/CompletionCommandFilter.cs:104

```
                copiedText = view.Selection.SelectedSpans[0].GetText();
            if(commandGroup==VSConstants.VSStd2K&&commandId==(uint)VSConstants.VSStd2KCmdID.TAB&&!broker.IsCompletionActive(view)&&SnippetInsertion.TryShortcut(view))return VSConstants.S_OK;
            if (IsCommitCommand(commandGroup, commandId) && TryCommitCodeAssistSelection())
            {
                return VSConstants.S_OK;
            }
```

 CodeAssist.Extension/CompletionCommandFilter.cs:217

```
        {
            if(e.Key!=System.Windows.Input.Key.LeftShift&&e.Key!=System.Windows.Input.Key.RightShift){shiftCaret=-1;return;}
            if(!StartupUi.Preferences.InsertUnderscoreAfterM||!view.Selection.IsEmpty||view.IsClosed)return;
            var caret=view.Caret.Position.BufferPosition;var line=caret.GetContainingLine();var before=line.GetText().Substring(0,caret.Position-line.Start.Position);
            if(System.Text.RegularExpressions.Regex.IsMatch(before,@"(?:^|\W)m$")){shiftCaret=caret;shiftStarted=System.Diagnostics.Stopwatch.GetTimestamp();}
        }
        private void OnKeyUp(object sender,System.Windows.Input.KeyEventArgs e)
```

### 추가 실행 대조

- Shift 타이밍·연속입력·IME 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F048 · Multiple Clipboards

실행 동등성 미검증

### VA 문서의 요구

공식 문서

- Ctrl+Shift+V 최근 clipboard 메뉴

- 표시 개수는 별도 설정이며 해당 도움말은 수치를 명시하지 않음

### CA의 현재 범위·차이

기존 장부에 구체적인 구현 범위 설명 누락. 후보 파일 존재만 확인함.

### 현재 코드 근거 후보

 CodeAssist.Extension/EditorClipboardHistory.cs:7

```
    {
        internal static readonly ClipboardHistory History = new ClipboardHistory();
    }
}
```

### 추가 실행 대조

- capacity·중복·선택·표준paste·Undo 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F049 · Reset Editor Zoom

실행 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/coding-assistance/reset-editor-zoom)

- editor scope에서100%로 복원; Ctrl+0은 추천키 수락 조건

- VS2015+는 새 창의 global zoom도100%로 설정

### CA의 현재 범위·차이

기존 장부에 구체적인 구현 범위 설명 누락. 후보 파일 존재만 확인함.

### 현재 코드 근거 후보

 CodeAssist.Extension/NavigationCommands.cs:3

```
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
```

 CodeAssist.Extension/NavigationCommands.cs:63

```
                {
                    var view = components?.GetService<IVsEditorAdaptersFactoryService>()?.GetWpfTextView(adapter);
                    if (view != null && !view.IsClosed)
                    {
                        if (command == 0x011e)
                        {
                            var snapshot = view.TextSnapshot;
```

 CodeAssist.Extension/NavigationCommands.cs:102

```
                        }
                        else if (command == 0x010f) view.ZoomLevel = 100;
                        else if (command == 0x011a) WordHighlights.For(view).Activate();
                        else if (command == 0x0117)
                        {
                            var snippetDte = await package.GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                            if (snippetDte == null) return;
```

### 추가 실행 대조

- 추천키 충돌·창별/global zoom·focus

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F050 · Shorthand

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/coding-assistance/shorthand)

- 축소 목록에서 여러 부분 문자열로 후보 검색; acronym 우선과 최근 사용 영향

- 목록 밖에서는 첫 입력 글자로 시작하는 symbol로 제한; Ctrl+Space로 전체 completion 전환

### CA의 현재 범위·차이

축약어 추천 후보; IDE 필터 통과 미검증; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CodeAssistCompletionSource.cs:126

```
                candidates = MergeCandidates(
                    indexService.Index.Complete(prefix, 200,StartupUi.Preferences.AcronymCompletionEnabled,StartupUi.Preferences.ShorthandCompletionEnabled),
                    indexService.CompleteLive(prefix, 200, filePath).Where(item => !item.IsLocal ||
                        ResolveLocalVariableType(snapshot, item.Name, point.Value.Position) != null).ToArray());
            }
```

### 추가 실행 대조

- 부분 문자열·prefix 제한·약어 충돌·우선 선택

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F051 · Smart Select

실행 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/coding-assistance/smart-select)

- Extend/Shrink와 Block Extend/Shrink 네 명령; Shift+Alt+]/[ 및 Alt+]/[

- 초기 범위는 커서 위치·leading whitespace·statement/block에 따라 구분

- 상대적 작은 증가·항상 word 시작·case/underscore word 분할 옵션과 종속성

- 댓글/문자열에서도 일반 extend/shrink 가능; block 명령은 block 수준 유지

- 선택이 화면 높이를 넘으면 시작 위치를 알리는 임시 창

### CA의 현재 범위·차이

기존 장부에 구체적인 구현 범위 설명 누락. 후보 파일 존재만 확인함.

### 현재 코드 근거 후보

 CodeAssist.Extension/NavigationCommands.cs:69

```
                            var snapshot = view.TextSnapshot;
                            var selected = view.Selection.StreamSelectionSpan.SnapshotSpan;
                            if (selected.Length == 0) return;
                            var descending = new CheckBox { Content = "내림차순", Margin = new Thickness(10) };
                            var ignoreCase = new CheckBox { Content = "대소문자 무시", Margin = new Thickness(10) };
                            var sort = new Button { Content = "정렬", IsDefault = true, Margin = new Thickness(10) };
                            var panel = new StackPanel(); panel.Children.Add(descending); panel.Children.Add(ignoreCase); panel.Children.Add(sort);
```

 CodeAssist.Extension/NavigationCommands.cs:70

```
                            var selected = view.Selection.StreamSelectionSpan.SnapshotSpan;
                            if (selected.Length == 0) return;
                            var descending = new CheckBox { Content = "내림차순", Margin = new Thickness(10) };
                            var ignoreCase = new CheckBox { Content = "대소문자 무시", Margin = new Thickness(10) };
                            var sort = new Button { Content = "정렬", IsDefault = true, Margin = new Thickness(10) };
                            var panel = new StackPanel(); panel.Children.Add(descending); panel.Children.Add(ignoreCase); panel.Children.Add(sort);
                            var dialog = new Window { Title = "CodeAssist: Sort Selected Lines", Content = panel, Width = 350, Height = 220, WindowStartupLocation = WindowStartupLocation.CenterOwner };
```

 CodeAssist.Extension/NavigationCommands.cs:75

```
                            var panel = new StackPanel(); panel.Children.Add(descending); panel.Children.Add(ignoreCase); panel.Children.Add(sort);
                            var dialog = new Window { Title = "CodeAssist: Sort Selected Lines", Content = panel, Width = 350, Height = 220, WindowStartupLocation = WindowStartupLocation.CenterOwner };
                            var sortDte = await package.GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                            if (sortDte != null) new System.Windows.Interop.WindowInteropHelper(dialog).Owner = (IntPtr)sortDte.MainWindow.HWnd;
                            sort.Click += (s, e) => dialog.DialogResult = true;
                            dialog.PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) dialog.DialogResult = false; };
                            if (dialog.ShowDialog() != true || view.IsClosed || !ReferenceEquals(snapshot, view.TextSnapshot)) return;
```

### 추가 실행 대조

- 네 명령의 교차 사용 및 외부 선택 후 shrink 비교

- leading whitespace·문자열/주석·case/underscore 옵션 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F052 · Smart Suggestions

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/coding-assistance/smart-suggestions)

- 생성자·enum 대입/비교·인자 타입에 맞는 local 변수·주석 hashtag를 제안

- 타입 이름의 Smart Suggestions snippet으로 값을 확장; 인자 comma+space는 다음 제안 유도

### CA의 현재 범위·차이

bool·포인터 선언의 예상 타입 리터럴 추천; 전체 문맥 추천 아님; VA 동등성 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CodeAssistCompletionSource.cs:143

```
            if(!StartupUi.Preferences.SuggestionListsEnabled)return;
            var snippetItems=StartupUi.Preferences.SnippetSuggestionsEnabled&&!hasResolvedMemberContext&&!includeMatch.Success?SnippetInsertion.Suggestions(indexService.ProjectRoot,prefix).Select(t=>(Completion)new SnippetCompletion(t)).ToList():new List<Completion>();
            if(StartupUi.Preferences.SmartSuggestionsEnabled&&!hasResolvedMemberContext&&!includeMatch.Success)
                snippetItems.AddRange(SmartSuggestions.At(snapshot.GetText(),point.Value.Position).Where(value=>value.StartsWith(prefix,StringComparison.OrdinalIgnoreCase)).Select(value=>(Completion)new Completion(value,value,"CodeAssist expected-type literal",null,"literal")));
            if (candidates.Count == 0&&snippetItems.Count==0) return;
            var builtInSet = completionSets.FirstOrDefault(set =>
                !string.Equals(set.Moniker, "CodeAssistPreview", StringComparison.Ordinal) &&
```

 CodeAssist.Extension/CodeAssistCompletionSource.cs:144

```
            var snippetItems=StartupUi.Preferences.SnippetSuggestionsEnabled&&!hasResolvedMemberContext&&!includeMatch.Success?SnippetInsertion.Suggestions(indexService.ProjectRoot,prefix).Select(t=>(Completion)new SnippetCompletion(t)).ToList():new List<Completion>();
            if(StartupUi.Preferences.SmartSuggestionsEnabled&&!hasResolvedMemberContext&&!includeMatch.Success)
                snippetItems.AddRange(SmartSuggestions.At(snapshot.GetText(),point.Value.Position).Where(value=>value.StartsWith(prefix,StringComparison.OrdinalIgnoreCase)).Select(value=>(Completion)new Completion(value,value,"CodeAssist expected-type literal",null,"literal")));
            if (candidates.Count == 0&&snippetItems.Count==0) return;
            var builtInSet = completionSets.FirstOrDefault(set =>
                !string.Equals(set.Moniker, "CodeAssistPreview", StringComparison.Ordinal) &&
                set.Completions != null && set.Completions.Count > 0);
```

 CodeAssist.Extension/CodeAssistCompletionSource.cs:145

```
            if(StartupUi.Preferences.SmartSuggestionsEnabled&&!hasResolvedMemberContext&&!includeMatch.Success)
                snippetItems.AddRange(SmartSuggestions.At(snapshot.GetText(),point.Value.Position).Where(value=>value.StartsWith(prefix,StringComparison.OrdinalIgnoreCase)).Select(value=>(Completion)new Completion(value,value,"CodeAssist expected-type literal",null,"literal")));
            if (candidates.Count == 0&&snippetItems.Count==0) return;
            var builtInSet = completionSets.FirstOrDefault(set =>
                !string.Equals(set.Moniker, "CodeAssistPreview", StringComparison.Ordinal) &&
                set.Completions != null && set.Completions.Count > 0);
            var start = point.Value.Position - prefix.Length;
```

### 추가 실행 대조

- 타입/overload·enum·사용자 snippet·comma 입력

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F053 · Sort Selected Lines

실행 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/coding-assistance/sort-selected-lines)

- 부분 마지막 줄 포함

- 마지막 줄 EOF이면 EOL 추가

- 정렬 옵션 대화상자

### CA의 현재 범위·차이

기존 장부에 구체적인 구현 범위 설명 누락. 후보 파일 존재만 확인함.

### 현재 코드 근거 후보

 CodeAssist.Extension/NavigationCommands.cs:73

```
                            var ignoreCase = new CheckBox { Content = "대소문자 무시", Margin = new Thickness(10) };
                            var sort = new Button { Content = "정렬", IsDefault = true, Margin = new Thickness(10) };
                            var panel = new StackPanel(); panel.Children.Add(descending); panel.Children.Add(ignoreCase); panel.Children.Add(sort);
                            var dialog = new Window { Title = "CodeAssist: Sort Selected Lines", Content = panel, Width = 350, Height = 220, WindowStartupLocation = WindowStartupLocation.CenterOwner };
                            var sortDte = await package.GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                            if (sortDte != null) new System.Windows.Interop.WindowInteropHelper(dialog).Owner = (IntPtr)sortDte.MainWindow.HWnd;
                            sort.Click += (s, e) => dialog.DialogResult = true;
```

 CodeAssist.Extension/NavigationCommands.cs:74

```
                            var sort = new Button { Content = "정렬", IsDefault = true, Margin = new Thickness(10) };
                            var panel = new StackPanel(); panel.Children.Add(descending); panel.Children.Add(ignoreCase); panel.Children.Add(sort);
                            var dialog = new Window { Title = "CodeAssist: Sort Selected Lines", Content = panel, Width = 350, Height = 220, WindowStartupLocation = WindowStartupLocation.CenterOwner };
                            var sortDte = await package.GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                            if (sortDte != null) new System.Windows.Interop.WindowInteropHelper(dialog).Owner = (IntPtr)sortDte.MainWindow.HWnd;
                            sort.Click += (s, e) => dialog.DialogResult = true;
                            dialog.PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) dialog.DialogResult = false; };
```

 CodeAssist.Extension/NavigationCommands.cs:75

```
                            var panel = new StackPanel(); panel.Children.Add(descending); panel.Children.Add(ignoreCase); panel.Children.Add(sort);
                            var dialog = new Window { Title = "CodeAssist: Sort Selected Lines", Content = panel, Width = 350, Height = 220, WindowStartupLocation = WindowStartupLocation.CenterOwner };
                            var sortDte = await package.GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                            if (sortDte != null) new System.Windows.Interop.WindowInteropHelper(dialog).Owner = (IntPtr)sortDte.MainWindow.HWnd;
                            sort.Click += (s, e) => dialog.DialogResult = true;
                            dialog.PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) dialog.DialogResult = false; };
                            if (dialog.ShowDialog() != true || view.IsClosed || !ReferenceEquals(snapshot, view.TextSnapshot)) return;
```

### 추가 실행 대조

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F054 · Suggestion Lists

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/coding-assistance/suggestion-lists)

- 최근/주변 symbol과 code bits; C#에서는 IDE 목록에 병합하거나 VA 목록 사용

- symbol 정의 중에는 자동 선택하지 않고 Down으로 선택; parameter list 제안은 global scope 및 Tab/Enter commit

- 오른쪽이 알려진 symbol이면 유지하고 모르는 잔여 문자열은 제거

### CA의 현재 범위·차이

추천 목록 사용 토글 및 C++·스니펫·컴파일러 후보 연결; VA 동등성 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CodeAssistCompletionSource.cs:142

```
            if(!StartupUi.Preferences.SuggestionListsEnabled)return;
            var snippetItems=StartupUi.Preferences.SnippetSuggestionsEnabled&&!hasResolvedMemberContext&&!includeMatch.Success?SnippetInsertion.Suggestions(indexService.ProjectRoot,prefix).Select(t=>(Completion)new SnippetCompletion(t)).ToList():new List<Completion>();
            if(StartupUi.Preferences.SmartSuggestionsEnabled&&!hasResolvedMemberContext&&!includeMatch.Success)
                snippetItems.AddRange(SmartSuggestions.At(snapshot.GetText(),point.Value.Position).Where(value=>value.StartsWith(prefix,StringComparison.OrdinalIgnoreCase)).Select(value=>(Completion)new Completion(value,value,"CodeAssist expected-type literal",null,"literal")));
            if (candidates.Count == 0&&snippetItems.Count==0) return;
            var builtInSet = completionSets.FirstOrDefault(set =>
```

 CodeAssist.Extension/CodeAssistCompletionSource.cs:143

```
            if(!StartupUi.Preferences.SuggestionListsEnabled)return;
            var snippetItems=StartupUi.Preferences.SnippetSuggestionsEnabled&&!hasResolvedMemberContext&&!includeMatch.Success?SnippetInsertion.Suggestions(indexService.ProjectRoot,prefix).Select(t=>(Completion)new SnippetCompletion(t)).ToList():new List<Completion>();
            if(StartupUi.Preferences.SmartSuggestionsEnabled&&!hasResolvedMemberContext&&!includeMatch.Success)
                snippetItems.AddRange(SmartSuggestions.At(snapshot.GetText(),point.Value.Position).Where(value=>value.StartsWith(prefix,StringComparison.OrdinalIgnoreCase)).Select(value=>(Completion)new Completion(value,value,"CodeAssist expected-type literal",null,"literal")));
            if (candidates.Count == 0&&snippetItems.Count==0) return;
            var builtInSet = completionSets.FirstOrDefault(set =>
                !string.Equals(set.Moniker, "CodeAssistPreview", StringComparison.Ordinal) &&
```

 CodeAssist.Extension/CodeAssistCompletionSource.cs:144

```
            var snippetItems=StartupUi.Preferences.SnippetSuggestionsEnabled&&!hasResolvedMemberContext&&!includeMatch.Success?SnippetInsertion.Suggestions(indexService.ProjectRoot,prefix).Select(t=>(Completion)new SnippetCompletion(t)).ToList():new List<Completion>();
            if(StartupUi.Preferences.SmartSuggestionsEnabled&&!hasResolvedMemberContext&&!includeMatch.Success)
                snippetItems.AddRange(SmartSuggestions.At(snapshot.GetText(),point.Value.Position).Where(value=>value.StartsWith(prefix,StringComparison.OrdinalIgnoreCase)).Select(value=>(Completion)new Completion(value,value,"CodeAssist expected-type literal",null,"literal")));
            if (candidates.Count == 0&&snippetItems.Count==0) return;
            var builtInSet = completionSets.FirstOrDefault(set =>
                !string.Equals(set.Moniker, "CodeAssistPreview", StringComparison.Ordinal) &&
                set.Completions != null && set.Completions.Count > 0);
```

### 추가 실행 대조

- 정의/참조 문맥·C# 공급원·commit·우측 문자열

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F055 · Surround Selection

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- slash는 완전한 줄/복수 줄이면 line comment, 부분 줄이면 block comment 토글

- asterisk는 block comment 토글; parentheses/braces surround

- surround character가 있는 snippet 입력은 선택을 expansion으로 대체

### CA의 현재 범위·차이

/, *, (, { 감싸기; #·Snippet 문자 토글 미완료

### 현재 코드 근거 후보

 CodeAssist.Core/CppEditing.cs:8

```
    {
        public EditorTextChange(int start,int length,string text,int selectionStart,int selectionLength)
        {Start=start;Length=length;Text=text;SelectionStart=selectionStart;SelectionLength=selectionLength;}
        public int Start{get;} public int Length{get;} public string Text{get;}
        public int SelectionStart{get;} public int SelectionLength{get;}
    }
    public static class CppEditing
```

 CodeAssist.Core/CppEditing.cs:9

```
        public EditorTextChange(int start,int length,string text,int selectionStart,int selectionLength)
        {Start=start;Length=length;Text=text;SelectionStart=selectionStart;SelectionLength=selectionLength;}
        public int Start{get;} public int Length{get;} public string Text{get;}
        public int SelectionStart{get;} public int SelectionLength{get;}
    }
    public static class CppEditing
    {
```

 CodeAssist.Core/CppEditing.cs:11

```
        public int Start{get;} public int Length{get;} public string Text{get;}
        public int SelectionStart{get;} public int SelectionLength{get;}
    }
    public static class CppEditing
    {
        public static EditorTextChange Surround(string text,int start,int length,char key,bool spaceAfterSlash)
        {
```

 CodeAssist.Extension/CompletionCommandFilter.cs:102

```
                (commandId == (uint)VSConstants.VSStd97CmdID.Copy || commandId == (uint)VSConstants.VSStd97CmdID.Cut) &&
                !view.Selection.IsEmpty && view.Selection.SelectedSpans.Count == 1 && view.Selection.SelectedSpans[0].Length <= 1024 * 1024)
                copiedText = view.Selection.SelectedSpans[0].GetText();
            if(commandGroup==VSConstants.VSStd2K&&commandId==(uint)VSConstants.VSStd2KCmdID.TAB&&!broker.IsCompletionActive(view)&&SnippetInsertion.TryShortcut(view))return VSConstants.S_OK;
            if (IsCommitCommand(commandGroup, commandId) && TryCommitCodeAssistSelection())
            {
                return VSConstants.S_OK;
```

 CodeAssist.Extension/CompletionCommandFilter.cs:103

```
                !view.Selection.IsEmpty && view.Selection.SelectedSpans.Count == 1 && view.Selection.SelectedSpans[0].Length <= 1024 * 1024)
                copiedText = view.Selection.SelectedSpans[0].GetText();
            if(commandGroup==VSConstants.VSStd2K&&commandId==(uint)VSConstants.VSStd2KCmdID.TAB&&!broker.IsCompletionActive(view)&&SnippetInsertion.TryShortcut(view))return VSConstants.S_OK;
            if (IsCommitCommand(commandGroup, commandId) && TryCommitCodeAssistSelection())
            {
                return VSConstants.S_OK;
            }
```

 CodeAssist.Extension/CompletionCommandFilter.cs:105

```
            if(commandGroup==VSConstants.VSStd2K&&commandId==(uint)VSConstants.VSStd2KCmdID.TAB&&!broker.IsCompletionActive(view)&&SnippetInsertion.TryShortcut(view))return VSConstants.S_OK;
            if (IsCommitCommand(commandGroup, commandId) && TryCommitCodeAssistSelection())
            {
                return VSConstants.S_OK;
            }

            var hasTypedCharacter = TryGetTypedCharacter(commandGroup, commandId, input, out var typedCharacter);
```

### 추가 실행 대조

- 부분/전체/복수 줄·기존주석·snippet·Undo 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F056 · Surround With

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/coding-assistance/surround-with)

- 선택을 snippet 코드로 감싸며 메뉴와 지정 surround 문자로 접근

- 단순 Surround Selection과 별개이며 Outline과 editor에서 가용 메뉴가 다름

### CA의 현재 범위·차이

선택 영역 스니펫 감싸기; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/SnippetInsertion.cs:66

```
        }
        internal static void Show(IWpfTextView view, string file, string projectRoot, IntPtr owner, string projectFile = null, string solutionFile = null, bool surroundOnly = false, string shortcut = null, SnapshotSpan? replacement = null)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var snapshot = view.TextSnapshot;
            var selection = replacement ?? view.Selection.StreamSelectionSpan.SnapshotSpan;
            var templates = new List<SnippetTemplate>(BuiltIns);
```

 CodeAssist.Extension/SnippetInsertion.cs:82

```
            if(shortcut!=null)templates=templates.Where(t=>string.Equals(t.Shortcut,shortcut,StringComparison.Ordinal)).ToList();
            if(surroundOnly)templates=templates.Where(t=>t.Variables().Contains("selected")).ToList();
            if(templates.Count==0)return;
            CaptureEnvironment();
            templates = templates.Select(template => template.WithEnvironment(name => environment.TryGetValue(name, out var value) ? value : "")).ToList();
            var reserved = SnippetReservedValues.Create(file, shortcut==null?selection.GetText():"", DateTime.Now, System.Globalization.CultureInfo.CurrentCulture);
            SnippetReservedValues.AddWorkspace(reserved, projectFile, solutionFile, Guid.NewGuid(), System.Globalization.CultureInfo.CurrentCulture);
```

### 추가 실행 대조

- 문자·메뉴·다중 snippet·선택/Undo

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F057 · Column Indicator

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-understanding/column-indicator)

- 지정 column의 뒤쪽 경계에 표시; 위치/색 옵션

- 가변폭 글꼴은 m 글자 폭의 배수로 위치 산정

### CA의 현재 범위·차이

설정 가능한 컬럼 기준선; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/EditorGuides.cs:46

```
    [Export(typeof(IWpfTextViewCreationListener))][ContentType("C/C++")][TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class ColumnGuideProvider:IWpfTextViewCreationListener
    {
        [Export(typeof(AdornmentLayerDefinition))][Name("CodeAssist Column Guide")][Order(After=PredefinedAdornmentLayers.Selection,Before=PredefinedAdornmentLayers.Text)]
        internal AdornmentLayerDefinition Layer=null;
        public void TextViewCreated(IWpfTextView view)=>view.Properties.GetOrCreateSingletonProperty(()=>new ColumnGuide(view));
    }
```

 CodeAssist.Extension/EditorGuides.cs:48

```
    {
        [Export(typeof(AdornmentLayerDefinition))][Name("CodeAssist Column Guide")][Order(After=PredefinedAdornmentLayers.Selection,Before=PredefinedAdornmentLayers.Text)]
        internal AdornmentLayerDefinition Layer=null;
        public void TextViewCreated(IWpfTextView view)=>view.Properties.GetOrCreateSingletonProperty(()=>new ColumnGuide(view));
    }
    internal sealed class ColumnGuide
    {
```

 CodeAssist.Extension/EditorGuides.cs:50

```
        internal AdornmentLayerDefinition Layer=null;
        public void TextViewCreated(IWpfTextView view)=>view.Properties.GetOrCreateSingletonProperty(()=>new ColumnGuide(view));
    }
    internal sealed class ColumnGuide
    {
        private readonly IWpfTextView view;private readonly IAdornmentLayer layer;
        internal ColumnGuide(IWpfTextView view){this.view=view;layer=view.GetAdornmentLayer("CodeAssist Column Guide");view.LayoutChanged+=Layout;view.Closed+=Closed;StartupUi.SettingsApplied+=Update;Update();}
```

### 추가 실행 대조

- 가변/고정폭·zoom·theme·가로 스크롤

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F058 · Context Field

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-understanding/context-field)

- symbol 문맥·scope 표시, unknown이면 비움; global scope는 현재 파일명

- field 폭 조절·scope 표시 축소·method dropdown·scope arrows·복사 메뉴

### CA의 현재 범위·차이

지연 갱신 소유 타입 문맥 필드; 전체 VA 문맥 UI와 다름; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/ContextBar.cs:17

```
    [Export(typeof(IWpfTextViewMarginProvider))]
    [Name("CodeAssist Context Bar")]
    [MarginContainer(PredefinedMarginNames.Top)]
    [ContentType("C/C++")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class ContextBarProvider : IWpfTextViewMarginProvider
    {
```

 CodeAssist.Extension/ContextBar.cs:21

```
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class ContextBarProvider : IWpfTextViewMarginProvider
    {
        [Import] internal ITextDocumentFactoryService Documents=null;
        [Import] internal ProjectIndexService Indexes=null;
        public IWpfTextViewMargin CreateMargin(IWpfTextViewHost host,IWpfTextViewMargin parent)
        {
```

 CodeAssist.Extension/ContextBar.cs:28

```
            Documents.TryGetTextDocument(host.TextView.TextDataModel.DocumentBuffer,out var document);
            return new ContextBarMargin(host.TextView,document?.FilePath,Indexes);
        }
    }
    internal sealed class ContextBarMargin : IWpfTextViewMargin
    {
        private const string Name="CodeAssist Context Bar";
```

### 추가 실행 대조

- 동명 scope·unknown·global·resize·IME/Unicode

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F059 · Definition Field

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-understanding/definition-field)

- symbol 정의·scope 상세·include 실제 경로 표시; 복수정의 목록은 정보용으로 선택해도 이동 안함

- unknown은 비움; typedef/macro를 적당히 평가하며 aggressive 평가 옵션 별도

### CA의 현재 범위·차이

커서 단어의 인덱스 서명 표시; 오버로드 의미 선택 미구현; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/ContextBar.cs:36

```
        private readonly string filePath;
        private ITextSnapshot definitionSnapshot;
        private System.Collections.Generic.IReadOnlyList<IndexedSymbol> documentSymbols;
        private readonly DockPanel panel=new DockPanel { Height=28,LastChildFill=true };
        private readonly ComboBox methods=CreateMethodSelector();
        internal static ComboBox CreateMethodSelector()
        {
```

 CodeAssist.Extension/ContextBar.cs:49

```
        private readonly ProjectIndexService indexes;
        private readonly TextBlock contextField=new TextBlock {Margin=new Thickness(8,0,12,0),TextTrimming=TextTrimming.CharacterEllipsis};
        private readonly TextBlock definitionField=new TextBlock {Margin=new Thickness(8,0,0,0),TextTrimming=TextTrimming.CharacterEllipsis};
        private readonly System.Windows.Threading.DispatcherTimer refresh=new System.Windows.Threading.DispatcherTimer {Interval=TimeSpan.FromMilliseconds(180)};
        private bool disposed;
        private ITextSnapshot methodSnapshot;
        internal ContextBarMargin(IWpfTextView view,string path,ProjectIndexService indexes=null)
```

 CodeAssist.Extension/ContextBar.cs:50

```
        private readonly TextBlock contextField=new TextBlock {Margin=new Thickness(8,0,12,0),TextTrimming=TextTrimming.CharacterEllipsis};
        private readonly TextBlock definitionField=new TextBlock {Margin=new Thickness(8,0,0,0),TextTrimming=TextTrimming.CharacterEllipsis};
        private readonly System.Windows.Threading.DispatcherTimer refresh=new System.Windows.Threading.DispatcherTimer {Interval=TimeSpan.FromMilliseconds(180)};
        private bool disposed;
        private ITextSnapshot methodSnapshot;
        internal ContextBarMargin(IWpfTextView view,string path,ProjectIndexService indexes=null)
        {
```

### 추가 실행 대조

- 복수 정의·include 경로·macro/typedef·복사

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F060 · Enhanced Syntax Coloring

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-understanding/enhanced-syntax-coloring)

- light/dark 기본 세트를 설치 시 IDE 설정으로 결정; 대비 부족 시 black/white fallback

- IDE coloring보다 우선; source/listbox/tool window 적용 선택 및 theme 인식 영향

- 추가 요소·배경색·reset 경로는 VS 버전별 차이

- 색은 심볼 사용 앞뒤 문자 영향을 받으며 정의가 항상 결정 요인은 아님

- C/C++ macro는 해당 header include 여부와 별개로 global 문맥으로 취급

### CA의 현재 범위·차이

분류기 구현과 현재 화면의 색상은 확인했습니다. 범용 문법·테마·기본색 전수 동등성은 미검증입니다.

### 현재 코드 근거 후보

 CodeAssist.Extension/CppSemanticClassifier.cs:13

```
{
    internal static class CppSemanticClassification
    {
        public const string TypeName = "CodeAssist C++ Type";
        public const string VariableName = "CodeAssist C++ Variable";
        public const string FunctionName = "CodeAssist C++ Function";
```

 CodeAssist.Extension/CppSemanticClassifier.cs:36

```
        [Order(After = Priority.High)]
        internal sealed class TypeFormat : ClassificationFormatDefinition
        {
            public TypeFormat()
            {
                DisplayName = "CodeAssist C++ Type";
                ForegroundColor = Color.FromRgb(255, 215, 0);
```

 CodeAssist.Extension/CppSemanticClassifier.cs:50

```
        [Order(After = Priority.High)]
        internal sealed class VariableFormat : ClassificationFormatDefinition
        {
            public VariableFormat()
            {
                DisplayName = "CodeAssist C++ Variable";
                ForegroundColor = Color.FromRgb(189, 183, 107);
```

### 추가 실행 대조

- light/dark·unknown theme·contrast fallback 비교

- IDE reset·추가 요소·창별 적용 비교

- 동명 다른 종류 심볼 및 include 없는 macro 색 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F061 · Local Symbols in Bold

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-understanding/local-symbols-in-bold)

- enhanced coloring이 필요하며 현재 파일에 정의된 이름에 bold 적용

- semantic scope와 별개로 동명 occurrence 모두 bold 가능; inherited listbox bold와 다른 옵션

### CA의 현재 범위·차이

렉시컬 지역 변수 식별자를 굵게 표시; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CppSemanticClassifier.cs:100

```
        private readonly IClassificationType functionClassification;
        private readonly IClassificationTypeRegistryService registry;private readonly ProjectIndexService indexes;private CppReferenceContext localContext;
        private ITextSnapshot cachedSnapshot;
        private IReadOnlyList<SemanticToken> cachedTokens = Array.Empty<SemanticToken>();

        public CppSemanticClassifier(ITextBuffer textBuffer, IClassificationType typeClassification, IClassificationType variableClassification, IClassificationType functionClassification,IClassificationTypeRegistryService registry=null,ProjectIndexService indexes=null)
        {
```

 CodeAssist.Extension/CppSemanticClassifier.cs:143

```
                        : variableClassification;
                if(StartupUi.Preferences.LocalSymbolsBold&&token.Kind==SemanticTokenKind.Variable&&localContext?.DeclarationAt(token.Name,token.Start)!=null)classification=registry?.GetClassificationType("CodeAssist Local Bold")??classification;
                else if(StartupUi.Preferences.SystemSymbolsItalic&&indexes!=null&&registry!=null)
                {
                    var definitions=indexes.Index.FindDefinitions(token.Name);
                    if(definitions.Count>0&&definitions.All(item=>SystemPath(item.FilePath)))classification=registry.GetClassificationType(token.Kind==SemanticTokenKind.Type?"CodeAssist System Type":token.Kind==SemanticTokenKind.Function?"CodeAssist System Function":"CodeAssist System Variable")??classification;
                }
```

 CodeAssist.Extension/CppSemanticClassifier.cs:164

```
            cachedTokens = CppSemanticParser.Parse(text);
            localContext=StartupUi.Preferences.LocalSymbolsBold&&snapshot.Length<=512*1024?new CppReferenceContext(text):null;
            cachedSnapshot = snapshot;
        }

        private static bool SystemPath(string path)=>!string.IsNullOrEmpty(path)&&(path.IndexOf(@"\Program Files",StringComparison.OrdinalIgnoreCase)>=0||path.IndexOf("/usr/include/",StringComparison.Ordinal)>=0||path.IndexOf(@"\Engine\Source\",StringComparison.OrdinalIgnoreCase)>=0);
        private void OnBufferChanged(object sender, TextContentChangedEventArgs e)
```

### 추가 실행 대조

- 동명 다른 scope·다른 파일의 class member·theme

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F062 · Highlight Find Results

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- IDE find 후 editor matches 강조; ESC로 해제; IDE Find Result(VA) 배경색 사용

### CA의 현재 범위·차이

검색 결과 버퍼에서 VS FindWhat 리터럴 강조; 정규식 검색·정확한 콘텐츠 타입 동작 미검증; VA 동등성 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/ResultHighlighting.cs:13

```
{
    internal static class ResultStyles
    {
        [Export(typeof(ClassificationTypeDefinition))][Name("CodeAssist Output File")]internal static ClassificationTypeDefinition File=null;
        [Export(typeof(ClassificationTypeDefinition))][Name("CodeAssist Find Match")]internal static ClassificationTypeDefinition Match=null;
        [Export(typeof(EditorFormatDefinition))][ClassificationType(ClassificationTypeNames="CodeAssist Output File")][Name("CodeAssist Output File")][UserVisible(true)]
        internal sealed class FileFormat:ClassificationFormatDefinition {public FileFormat(){DisplayName="CodeAssist Output File";ForegroundColor=Color.FromRgb(80,170,255);}}
```

 CodeAssist.Extension/ResultHighlighting.cs:16

```
        [Export(typeof(ClassificationTypeDefinition))][Name("CodeAssist Output File")]internal static ClassificationTypeDefinition File=null;
        [Export(typeof(ClassificationTypeDefinition))][Name("CodeAssist Find Match")]internal static ClassificationTypeDefinition Match=null;
        [Export(typeof(EditorFormatDefinition))][ClassificationType(ClassificationTypeNames="CodeAssist Output File")][Name("CodeAssist Output File")][UserVisible(true)]
        internal sealed class FileFormat:ClassificationFormatDefinition {public FileFormat(){DisplayName="CodeAssist Output File";ForegroundColor=Color.FromRgb(80,170,255);}}
        [Export(typeof(EditorFormatDefinition))][ClassificationType(ClassificationTypeNames="CodeAssist Find Match")][Name("CodeAssist Find Match")][UserVisible(true)]
        internal sealed class MatchFormat:ClassificationFormatDefinition {public MatchFormat(){DisplayName="CodeAssist Find Match";BackgroundColor=Color.FromRgb(105,85,30);}}
    }
```

 CodeAssist.Extension/ResultHighlighting.cs:19

```
        internal sealed class FileFormat:ClassificationFormatDefinition {public FileFormat(){DisplayName="CodeAssist Output File";ForegroundColor=Color.FromRgb(80,170,255);}}
        [Export(typeof(EditorFormatDefinition))][ClassificationType(ClassificationTypeNames="CodeAssist Find Match")][Name("CodeAssist Find Match")][UserVisible(true)]
        internal sealed class MatchFormat:ClassificationFormatDefinition {public MatchFormat(){DisplayName="CodeAssist Find Match";BackgroundColor=Color.FromRgb(105,85,30);}}
    }
    [Export(typeof(IClassifierProvider))][ContentType("text")]
    internal sealed class ResultHighlightProvider:IClassifierProvider
    {
```

### 추가 실행 대조

- 새검색·ESC·문서전환·theme 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F063 · Highlight Matching Words

실행 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-understanding/highlight-matching-words)

- 문자열/주석 안 active word에서 시작하고 파일 내 동일 단어에도 적용; scope 무시

- 대소문자 일치·전체 단어만; read reference highlight 색 공유

### CA의 현재 범위·차이

기존 장부에 구체적인 구현 범위 설명 누락. 후보 파일 존재만 확인함.

### 현재 코드 근거 후보

 CodeAssist.Extension/WordHighlights.cs:17

```
    [Export(typeof(EditorFormatDefinition))]
    [Name(WordHighlights.Marker)]
    [UserVisible(true)]
    internal sealed class WordHighlightFormat : MarkerFormatDefinition
    {
        public WordHighlightFormat() { DisplayName = "CodeAssist Find Selected"; BackgroundColor = Color.FromRgb(95, 85, 35); }
    }
```

 CodeAssist.Extension/WordHighlights.cs:19

```
    [UserVisible(true)]
    internal sealed class WordHighlightFormat : MarkerFormatDefinition
    {
        public WordHighlightFormat() { DisplayName = "CodeAssist Find Selected"; BackgroundColor = Color.FromRgb(95, 85, 35); }
    }
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("C/C++")]
```

 CodeAssist.Extension/WordHighlights.cs:21

```
    {
        public WordHighlightFormat() { DisplayName = "CodeAssist Find Selected"; BackgroundColor = Color.FromRgb(95, 85, 35); }
    }
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("C/C++")]
    [TagType(typeof(TextMarkerTag))]
    internal sealed class WordHighlightProvider : IViewTaggerProvider
```

### 추가 실행 대조

- 주석/문자열에서 동명 다른 scope·대소문자·부분 단어 비교

- code로 이동 및 선택·ESC 수명 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F064 · Highlight References to Symbol Under Cursor

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-understanding/highlight-references-to-symbol-under-cursor)

- 같은 semantic context 참조만; read/write 구분

- 공백 이동 시 유지·ESC 임시 해제·project 외 파일 지원

- Find References 색 공유; 문서는 일반적으로 write MistyRose/read LightCyan으로 설명하며 실제 테마별 값 별도 검증

- IDE 유사 기능을 disable/re-enable할 기회 제공

### CA의 현재 범위·차이

지연·단일 작업·캐시를 적용한 커서 심볼 렉시컬 참조 강조; 컴파일러 동등성 미검증; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/ReferenceHighlighting.cs:17

```
    [Export(typeof(IViewTaggerProvider))][ContentType("C/C++")][TagType(typeof(TextMarkerTag))]
    internal sealed class ReferenceHighlightProvider:IViewTaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextView view,ITextBuffer buffer) where T:ITag=>view.TextBuffer==buffer?view.Properties.GetOrCreateSingletonProperty(()=>new ReferenceHighlighting(view)) as ITagger<T>:null;
    }
    internal sealed class ReferenceHighlighting:ITagger<TextMarkerTag>
    {
```

 CodeAssist.Extension/ReferenceHighlighting.cs:19

```
    {
        public ITagger<T> CreateTagger<T>(ITextView view,ITextBuffer buffer) where T:ITag=>view.TextBuffer==buffer?view.Properties.GetOrCreateSingletonProperty(()=>new ReferenceHighlighting(view)) as ITagger<T>:null;
    }
    internal sealed class ReferenceHighlighting:ITagger<TextMarkerTag>
    {
        private readonly ITextView view;private readonly DispatcherTimer timer=new DispatcherTimer {Interval=TimeSpan.FromMilliseconds(220)};
        private ITextSnapshot parsed,published;private CppReferenceContext context;private int[] offsets=Array.Empty<int>();private int length;private bool closed,running;
```

 CodeAssist.Extension/ReferenceHighlighting.cs:21

```
    }
    internal sealed class ReferenceHighlighting:ITagger<TextMarkerTag>
    {
        private readonly ITextView view;private readonly DispatcherTimer timer=new DispatcherTimer {Interval=TimeSpan.FromMilliseconds(220)};
        private ITextSnapshot parsed,published;private CppReferenceContext context;private int[] offsets=Array.Empty<int>();private int length;private bool closed,running;
        internal ReferenceHighlighting(ITextView view){this.view=view;view.Caret.PositionChanged+=Caret;view.TextBuffer.Changed+=Changed;view.Closed+=Closed;timer.Tick+=Tick;StartupUi.SettingsApplied+=Schedule;if(view is IWpfTextView wpf)wpf.VisualElement.PreviewMouseLeftButtonUp+=Mouse;Schedule();}
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
```

### 추가 실행 대조

- 동명 shadowing·read/write·공백·ESC 비교

- IDE 중복 표시·테마 및 프로젝트 외 파일 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F065 · Parameter Info

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-understanding/parameter-info)

- method 뒤 여는 괄호 또는 Ctrl+Shift+Space; 현재 인자 bold·위/아래로 overload 탐색

- C# 공급원은 default IntelliSense; disable 반영에는 IDE 재시작 조건

### CA의 현재 범위·차이

컴파일러 signatureHelp 결과 및 현재 인수 번호 표시; 타이핑 자동 팝업 없음; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/UtilityCommands.cs:55

```
                var picker=new Microsoft.Win32.OpenFileDialog {Title="CodeAssist: Preview File",Filter="C/C++ files|*.h;*.hpp;*.cpp;*.c;*.cc;*.inl|All files|*.*"};if(picker.ShowDialog()!=true)return;
                if(new FileInfo(picker.FileName).Length>2*1024*1024)throw new InvalidOperationException("Preview is limited to 2 MiB.");var text=await System.Threading.Tasks.Task.Run(()=>File.ReadAllText(picker.FileName));await ShowAsync(package,Path.GetFileName(picker.FileName),new FlowDocumentScrollViewer {Document=ColoredDocument(text),IsToolBarVisible=true});return;
            }
            if(id==0x194)
            {
                var document=dte.ActiveDocument?.Object("TextDocument") as EnvDTE.TextDocument;if(document==null)return;var text=document.StartPoint.CreateEditPoint().GetText(document.EndPoint);
                if(text.Length>2*1024*1024)throw new InvalidOperationException("Color printing is limited to 2 MiB.");var dialog=new PrintDialog();if(dialog.ShowDialog()==true){var documentToPrint=ColoredDocument(text);documentToPrint.PageWidth=dialog.PrintableAreaWidth;documentToPrint.ColumnWidth=dialog.PrintableAreaWidth;dialog.PrintDocument(((IDocumentPaginatorSource)documentToPrint).DocumentPaginator,"CodeAssist: "+dte.ActiveDocument.Name);}return;
```

 CodeAssist.Extension/UtilityCommands.cs:71

```
                if(!(help?["signatures"] is JArray signatures)||signatures.Count==0)throw new InvalidOperationException("No compiler signature is available at the caret. Configure a compilation database and place the caret in an argument list.");
                var lines=new StringBuilder("Active parameter: "+((int?)help["activeParameter"]+1)+Environment.NewLine);
                foreach(var signature in signatures)lines.AppendLine((string)signature["label"]).AppendLine((string)(signature["documentation"] is JObject documentation?documentation["value"]:signature["documentation"]));
                await ShowAsync(package,"Parameter Info",new TextBox {Text=lines.ToString(),IsReadOnly=true,AcceptsReturn=true,FontFamily=new FontFamily("Consolas"),VerticalScrollBarVisibility=ScrollBarVisibility.Auto});
            }
        }
        internal static FlowDocument ColoredDocument(string text)
```

 CodeAssist.Extension/UtilityCommands.cs:73

```
                foreach(var signature in signatures)lines.AppendLine((string)signature["label"]).AppendLine((string)(signature["documentation"] is JObject documentation?documentation["value"]:signature["documentation"]));
                await ShowAsync(package,"Parameter Info",new TextBox {Text=lines.ToString(),IsReadOnly=true,AcceptsReturn=true,FontFamily=new FontFamily("Consolas"),VerticalScrollBarVisibility=ScrollBarVisibility.Auto});
            }
        }
        internal static FlowDocument ColoredDocument(string text)
        {
            var document=new FlowDocument {FontFamily=new FontFamily("Consolas"),FontSize=12,PagePadding=new Thickness(24),Background=Brushes.White,Foreground=Brushes.Black};var paragraph=new Paragraph {Margin=new Thickness(0)};document.Blocks.Add(paragraph);var cursor=0;
```

### 추가 실행 대조

- 중간 인자 복귀·overload·macro·C#·재시작

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F066 · Print in Color

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-understanding/print-in-color)

- VS2015+ print에 VA 색을 포함하며 IDE 버전별 지원이 다름

- 출력 selection만 따로 파싱하므로 잘린 문맥의 색이 원본과 다를 수 있음

### CA의 현재 범위·차이

CodeAssist 분류색을 이용한 사용자 확인 후 컬러 인쇄; 주석·문자열 전체 VS 색상 재현 아님; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/UtilityCommands.cs:55

```
                var picker=new Microsoft.Win32.OpenFileDialog {Title="CodeAssist: Preview File",Filter="C/C++ files|*.h;*.hpp;*.cpp;*.c;*.cc;*.inl|All files|*.*"};if(picker.ShowDialog()!=true)return;
                if(new FileInfo(picker.FileName).Length>2*1024*1024)throw new InvalidOperationException("Preview is limited to 2 MiB.");var text=await System.Threading.Tasks.Task.Run(()=>File.ReadAllText(picker.FileName));await ShowAsync(package,Path.GetFileName(picker.FileName),new FlowDocumentScrollViewer {Document=ColoredDocument(text),IsToolBarVisible=true});return;
            }
            if(id==0x194)
            {
                var document=dte.ActiveDocument?.Object("TextDocument") as EnvDTE.TextDocument;if(document==null)return;var text=document.StartPoint.CreateEditPoint().GetText(document.EndPoint);
                if(text.Length>2*1024*1024)throw new InvalidOperationException("Color printing is limited to 2 MiB.");var dialog=new PrintDialog();if(dialog.ShowDialog()==true){var documentToPrint=ColoredDocument(text);documentToPrint.PageWidth=dialog.PrintableAreaWidth;documentToPrint.ColumnWidth=dialog.PrintableAreaWidth;dialog.PrintDocument(((IDocumentPaginatorSource)documentToPrint).DocumentPaginator,"CodeAssist: "+dte.ActiveDocument.Name);}return;
```

 CodeAssist.Extension/UtilityCommands.cs:60

```
                var document=dte.ActiveDocument?.Object("TextDocument") as EnvDTE.TextDocument;if(document==null)return;var text=document.StartPoint.CreateEditPoint().GetText(document.EndPoint);
                if(text.Length>2*1024*1024)throw new InvalidOperationException("Color printing is limited to 2 MiB.");var dialog=new PrintDialog();if(dialog.ShowDialog()==true){var documentToPrint=ColoredDocument(text);documentToPrint.PageWidth=dialog.PrintableAreaWidth;documentToPrint.ColumnWidth=dialog.PrintableAreaWidth;dialog.PrintDocument(((IDocumentPaginatorSource)documentToPrint).DocumentPaginator,"CodeAssist: "+dte.ActiveDocument.Name);}return;
            }
            if(id==0x195)
            {
                var document=dte.ActiveDocument;var selection=document?.Selection as EnvDTE.TextSelection;var textDocument=document?.Object("TextDocument") as EnvDTE.TextDocument;if(selection==null||textDocument==null)return;
                var root=Path.GetDirectoryName(document.FullName);while(root!=null&&!File.Exists(Path.Combine(root,"compile_commands.json"))&&!File.Exists(Path.Combine(root,"compile_flags.txt")))root=Path.GetDirectoryName(root);
```

 CodeAssist.Extension/UtilityCommands.cs:76

```
        }
        internal static FlowDocument ColoredDocument(string text)
        {
            var document=new FlowDocument {FontFamily=new FontFamily("Consolas"),FontSize=12,PagePadding=new Thickness(24),Background=Brushes.White,Foreground=Brushes.Black};var paragraph=new Paragraph {Margin=new Thickness(0)};document.Blocks.Add(paragraph);var cursor=0;
            foreach(var token in CppSemanticParser.Parse(text).OrderBy(t=>t.Start))
            {if(token.Start<cursor)continue;if(token.Start>cursor)paragraph.Inlines.Add(new Run(text.Substring(cursor,token.Start-cursor)));var color=token.Kind==SemanticTokenKind.Type?StartupUi.Preferences.TypeColor:token.Kind==SemanticTokenKind.Function?StartupUi.Preferences.FunctionColor:StartupUi.Preferences.VariableColor;paragraph.Inlines.Add(new Run(text.Substring(token.Start,token.Length)){Foreground=(Brush)new BrushConverter().ConvertFromString(color)});cursor=token.Start+token.Length;}
            if(cursor<text.Length)paragraph.Inlines.Add(new Run(text.Substring(cursor)));return document;
```

### 추가 실행 대조

- 전체/일부 출력·theme·VS버전·context

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F067 · Quick Info

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-understanding/quick-info)

- IDE tooltip 보완/대체·source 주석·닫는 brace/구조 keyword 문맥·Doxygen 형식 옵션

- IDE 버전별 공급원과 중복주석 조건이 다름; mouse 이동/ESC로 해제

### CA의 현재 범위·차이

인덱스 후보 툴팁; 컴파일러 문맥 선택 미완료

### 현재 코드 근거 후보

 CodeAssist.Extension/SymbolQuickInfo.cs:14

```
{
    [Export(typeof(IAsyncQuickInfoSourceProvider))]
    [Name("CodeAssist Symbol Quick Info")]
    [ContentType("C/C++")]
    [Order(After=Priority.Default)]
    internal sealed class SymbolQuickInfoProvider : IAsyncQuickInfoSourceProvider
    {
```

 CodeAssist.Extension/SymbolQuickInfo.cs:15

```
    [Export(typeof(IAsyncQuickInfoSourceProvider))]
    [Name("CodeAssist Symbol Quick Info")]
    [ContentType("C/C++")]
    [Order(After=Priority.Default)]
    internal sealed class SymbolQuickInfoProvider : IAsyncQuickInfoSourceProvider
    {
        [Import] internal ProjectIndexService Indexes=null;
```

 CodeAssist.Extension/SymbolQuickInfo.cs:18

```
    [Order(After=Priority.Default)]
    internal sealed class SymbolQuickInfoProvider : IAsyncQuickInfoSourceProvider
    {
        [Import] internal ProjectIndexService Indexes=null;
        public IAsyncQuickInfoSource TryCreateQuickInfoSource(ITextBuffer buffer) => new SymbolQuickInfoSource(buffer,Indexes);
    }
    internal sealed class SymbolQuickInfoSource : IAsyncQuickInfoSource
```

### 추가 실행 대조

- VS버전·IDE 병합/대체·source/header 주석·macro·ESC

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F068 · System Symbols in Italics

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-understanding/system-symbols-in-italics)

- system include와 Program Files의 symbol 등은 italics 후보이며 project override는 구별

- system 선언 DB는 solutions 간 공유 읽기 전용이며 처음 포함한 solution에서 파싱

### CA의 현재 범위·차이

외부 SDK·엔진 경로 심볼 기울임; SDK 경로 휴리스틱; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CppSemanticClassifier.cs:1

```
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
```

 CodeAssist.Extension/CppSemanticClassifier.cs:5

```
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Media;
using CodeAssist.Core;
```

 CodeAssist.Extension/CppSemanticClassifier.cs:6

```
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Media;
using CodeAssist.Core;

namespace CodeAssist.Extension
```

### 추가 실행 대조

- 동명 override·NuGet/Vcpkg·ProgramFiles·재파싱

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F069 · Convert Dot to ->

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- 적절한 문맥에서 dot를 arrow로 바꿈; operator-> smart object 변형은 별도 옵션

- Delete/Undo/Backspace로 철회

### CA의 현재 범위·차이

확정 포인터 타입에서 점 입력을 화살표로 변환; 오버로드 operator-> 제외; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CompletionCommandFilter.cs:210

```
            var text=snapshot.GetText();var caret=view.Caret.Position.BufferPosition.Position;var word=CppAutomaticEdits.WordBefore(text,caret);
            if(key=='.'&&preferences.ConvertDotToArrow&&word.Length>0)
            {var type=new CppReferenceContext(text).ResolveVariableType(word,caret)??indexes?.Index.ResolveVariableType(word);return ApplyAutomatic(CppAutomaticEdits.DotToArrow(text,caret,type),"CodeAssist Dot to Arrow");}
            if(preferences.RepairTypeCase&&word.Length>0&&new CppReferenceContext(text).ResolveVariableType(word,caret)==null)ApplyAutomatic(CppAutomaticEdits.RepairCase(text,caret,indexes?.Index.FindCorrectTypeCasing(word)),"CodeAssist Repair Type Case");
            return false;
        }
        private void OnKeyDown(object sender,System.Windows.Input.KeyEventArgs e)
```

 CodeAssist.Extension/CompletionCommandFilter.cs:287

```
                var caret=view.Caret.Position.BufferPosition.Position;var snapshot=view.TextSnapshot;
                var indent=(!view.Options.GetOptionValue(DefaultOptions.ConvertTabsToSpacesOptionId))?"\t":new string(' ',Math.Max(1,Math.Min(16,view.Options.GetOptionValue(DefaultOptions.IndentSizeOptionId))));
                var contents=snapshot.GetText();
                var freshNativePair=CppPairing.IsFreshPair(before.GetText(),selected.Start,selected.Length,contents,caret,key);
                var planned=CppPairing.AfterOpening(freshNativePair?contents.Remove(caret,1):contents,caret,key,StartupUi.Preferences.ClosingBracePlacement,indent);
                if(EditorInputTrace.Enabled)WelcomeDocumentWindow.RecordEvent("PairInsertion owned="+(planned!=null)+" freshNative="+freshNativePair+" caret="+caret);
                if(planned!=null)
```

### 추가 실행 대조

- pointer·instance·smart object·철회 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F070 · Highlight Files in Output Window

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- build output에서 solution 파일 경로를 다른 파일 경로와 구별해 강조

### CA의 현재 범위·차이

출력·검색 버퍼의 C/C++ 파일 경로 분류색; 클릭 탐색은 VS 제공; VA 동등성 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/ResultHighlighting.cs:5

```
using System.Text.RegularExpressions;
using System.Windows.Media;
using CodeAssist.Core;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
```

 CodeAssist.Extension/ResultHighlighting.cs:15

```
    {
        [Export(typeof(ClassificationTypeDefinition))][Name("CodeAssist Output File")]internal static ClassificationTypeDefinition File=null;
        [Export(typeof(ClassificationTypeDefinition))][Name("CodeAssist Find Match")]internal static ClassificationTypeDefinition Match=null;
        [Export(typeof(EditorFormatDefinition))][ClassificationType(ClassificationTypeNames="CodeAssist Output File")][Name("CodeAssist Output File")][UserVisible(true)]
        internal sealed class FileFormat:ClassificationFormatDefinition {public FileFormat(){DisplayName="CodeAssist Output File";ForegroundColor=Color.FromRgb(80,170,255);}}
        [Export(typeof(EditorFormatDefinition))][ClassificationType(ClassificationTypeNames="CodeAssist Find Match")][Name("CodeAssist Find Match")][UserVisible(true)]
        internal sealed class MatchFormat:ClassificationFormatDefinition {public MatchFormat(){DisplayName="CodeAssist Find Match";BackgroundColor=Color.FromRgb(105,85,30);}}
```

 CodeAssist.Extension/ResultHighlighting.cs:17

```
        [Export(typeof(ClassificationTypeDefinition))][Name("CodeAssist Find Match")]internal static ClassificationTypeDefinition Match=null;
        [Export(typeof(EditorFormatDefinition))][ClassificationType(ClassificationTypeNames="CodeAssist Output File")][Name("CodeAssist Output File")][UserVisible(true)]
        internal sealed class FileFormat:ClassificationFormatDefinition {public FileFormat(){DisplayName="CodeAssist Output File";ForegroundColor=Color.FromRgb(80,170,255);}}
        [Export(typeof(EditorFormatDefinition))][ClassificationType(ClassificationTypeNames="CodeAssist Find Match")][Name("CodeAssist Find Match")][UserVisible(true)]
        internal sealed class MatchFormat:ClassificationFormatDefinition {public MatchFormat(){DisplayName="CodeAssist Find Match";BackgroundColor=Color.FromRgb(105,85,30);}}
    }
    [Export(typeof(IClassifierProvider))][ContentType("text")]
```

### 추가 실행 대조

- solution 내부/외부·상대/절대경로·출력갱신 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F071 · Highlight Matching Braces and Parentheses

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- caret가 scope 안에 있을 때 matching delimiter 강조

### CA의 현재 범위·차이

문자열·주석 제외 괄호 짝 강조; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/EditorGuides.cs:40

```
            if(!map.At(position,out partner)){position=caret-1;if(!map.At(position,out partner))yield break;}
            var p=StartupUi.Preferences;if(partner<0&&!p.HighlightMismatchingBraces||partner>=0&&!p.HighlightMatchingBraces)yield break;
            var tag=new TextMarkerTag(partner<0?"CodeAssist Mismatched Brace":"CodeAssist Matched Brace");
            foreach(var offset in partner<0?new[]{position}:new[]{position,partner}){var span=new SnapshotSpan(snapshot,offset,1);foreach(var requested in spans)if(requested.IntersectsWith(span)){yield return new TagSpan<TextMarkerTag>(span,tag);break;}}
        }
    }
    [Export(typeof(IWpfTextViewCreationListener))][ContentType("C/C++")][TextViewRole(PredefinedTextViewRoles.Document)]
```

 CodeAssist.Extension/EditorGuides.cs:60

```
            if(view.IsClosed)return;layer.RemoveAllAdornments();var p=StartupUi.Preferences;
            if(view.Options.GetOptionValue(DefaultWpfViewOptions.EnableHighlightCurrentLineId)!=p.HighlightCurrentLine)view.Options.SetOptionValue(DefaultWpfViewOptions.EnableHighlightCurrentLineId,p.HighlightCurrentLine);
            if(!p.ColumnIndicatorEnabled||view.ViewportHeight<=0)return;
            var x=view.FormattedLineSource.BaseIndentation+view.FormattedLineSource.ColumnWidth*p.ColumnIndicator-view.ViewportLeft;
            if(x<0||x>view.ViewportWidth)return;
            var line=new Line {X1=x,X2=x,Y1=0,Y2=view.ViewportHeight,Stroke=Brushes.Gray,Opacity=.5,StrokeThickness=1,IsHitTestVisible=false};
            layer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative,null,null,line,null);
```

### 추가 실행 대조

- 중첩·문자열/주석 delimiter·caret 경계 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F072 · Highlight Mismatching Braces and Parentheses

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- caret 문맥에서 균형이 맞지 않는 delimiter 강조

### CA의 현재 범위·차이

불일치·미닫힘 괄호 강조; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/EditorGuides.cs:40

```
            if(!map.At(position,out partner)){position=caret-1;if(!map.At(position,out partner))yield break;}
            var p=StartupUi.Preferences;if(partner<0&&!p.HighlightMismatchingBraces||partner>=0&&!p.HighlightMatchingBraces)yield break;
            var tag=new TextMarkerTag(partner<0?"CodeAssist Mismatched Brace":"CodeAssist Matched Brace");
            foreach(var offset in partner<0?new[]{position}:new[]{position,partner}){var span=new SnapshotSpan(snapshot,offset,1);foreach(var requested in spans)if(requested.IntersectsWith(span)){yield return new TagSpan<TextMarkerTag>(span,tag);break;}}
        }
    }
    [Export(typeof(IWpfTextViewCreationListener))][ContentType("C/C++")][TextViewRole(PredefinedTextViewRoles.Document)]
```

 CodeAssist.Extension/EditorGuides.cs:60

```
            if(view.IsClosed)return;layer.RemoveAllAdornments();var p=StartupUi.Preferences;
            if(view.Options.GetOptionValue(DefaultWpfViewOptions.EnableHighlightCurrentLineId)!=p.HighlightCurrentLine)view.Options.SetOptionValue(DefaultWpfViewOptions.EnableHighlightCurrentLineId,p.HighlightCurrentLine);
            if(!p.ColumnIndicatorEnabled||view.ViewportHeight<=0)return;
            var x=view.FormattedLineSource.BaseIndentation+view.FormattedLineSource.ColumnWidth*p.ColumnIndicator-view.ViewportLeft;
            if(x<0||x>view.ViewportWidth)return;
            var line=new Line {X1=x,X2=x,Y1=0,Y2=view.ViewportHeight,Stroke=Brushes.Gray,Opacity=.5,StrokeThickness=1,IsHitTestVisible=false};
            layer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative,null,null,line,null);
```

### 추가 실행 대조

- 불완전코드·여는/닫는짝·macro 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F073 · Repair Case

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- typing 중 symbol 대소문자 수정; 여러 교정 후보가 있으면 수정하지 않음

- C#은 suggestion/completion 목록 활성 필요

### CA의 현재 범위·차이

인덱스 기반 타입명 대소문자 교정; 지역 변수 교정 제외; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CompletionCommandFilter.cs:208

```
            var preferences=StartupUi.Preferences;var snapshot=view.TextSnapshot;if(snapshot.Length>512*1024)return false;
            if(key!='.'&&(!preferences.RepairTypeCase||char.IsLetterOrDigit(key)||key=='_'))return false;
            var text=snapshot.GetText();var caret=view.Caret.Position.BufferPosition.Position;var word=CppAutomaticEdits.WordBefore(text,caret);
            if(key=='.'&&preferences.ConvertDotToArrow&&word.Length>0)
            {var type=new CppReferenceContext(text).ResolveVariableType(word,caret)??indexes?.Index.ResolveVariableType(word);return ApplyAutomatic(CppAutomaticEdits.DotToArrow(text,caret,type),"CodeAssist Dot to Arrow");}
            if(preferences.RepairTypeCase&&word.Length>0&&new CppReferenceContext(text).ResolveVariableType(word,caret)==null)ApplyAutomatic(CppAutomaticEdits.RepairCase(text,caret,indexes?.Index.FindCorrectTypeCasing(word)),"CodeAssist Repair Type Case");
            return false;
```

 CodeAssist.Extension/CompletionCommandFilter.cs:212

```
            {var type=new CppReferenceContext(text).ResolveVariableType(word,caret)??indexes?.Index.ResolveVariableType(word);return ApplyAutomatic(CppAutomaticEdits.DotToArrow(text,caret,type),"CodeAssist Dot to Arrow");}
            if(preferences.RepairTypeCase&&word.Length>0&&new CppReferenceContext(text).ResolveVariableType(word,caret)==null)ApplyAutomatic(CppAutomaticEdits.RepairCase(text,caret,indexes?.Index.FindCorrectTypeCasing(word)),"CodeAssist Repair Type Case");
            return false;
        }
        private void OnKeyDown(object sender,System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key!=System.Windows.Input.Key.LeftShift&&e.Key!=System.Windows.Input.Key.RightShift){shiftCaret=-1;return;}
```

### 추가 실행 대조

- 단일/복수 후보·C# 조건·Undo 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F074 · Underline Spelling Errors

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- 주석과 문자열 spelling 표시; 문맥 메뉴에서 교정 또는 사전 추가

### CA의 현재 범위·차이

WPF 사전 기반 주석·문자열 소문자 단어 밑줄; OS 사전 유무·IDE 실행 미검증; VA 동등성 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/SpellingTagger.cs:17

```
    [Export(typeof(ITaggerProvider))][ContentType("C/C++")][TagType(typeof(IErrorTag))]
    internal sealed class SpellingProvider:ITaggerProvider
    {public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T:ITag=>buffer.Properties.GetOrCreateSingletonProperty(()=>new SpellingTagger(buffer)) as ITagger<T>;}
    internal sealed class SpellingTagger:ITagger<IErrorTag>
    {
        private readonly ITextBuffer buffer;private ITextSnapshot parsed;private string mask;private TextBox checker;private string language;
        private readonly Dictionary<string,string> cache=new Dictionary<string,string>(StringComparer.Ordinal);
```

 CodeAssist.Extension/SpellingTagger.cs:18

```
    internal sealed class SpellingProvider:ITaggerProvider
    {public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T:ITag=>buffer.Properties.GetOrCreateSingletonProperty(()=>new SpellingTagger(buffer)) as ITagger<T>;}
    internal sealed class SpellingTagger:ITagger<IErrorTag>
    {
        private readonly ITextBuffer buffer;private ITextSnapshot parsed;private string mask;private TextBox checker;private string language;
        private readonly Dictionary<string,string> cache=new Dictionary<string,string>(StringComparer.Ordinal);
        internal SpellingTagger(ITextBuffer buffer){this.buffer=buffer;buffer.Changed+=(s,e)=>{parsed=null;TagsChanged?.Invoke(this,new SnapshotSpanEventArgs(new SnapshotSpan(e.After,0,e.After.Length)));};}
```

 CodeAssist.Extension/SpellingTagger.cs:19

```
    {public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T:ITag=>buffer.Properties.GetOrCreateSingletonProperty(()=>new SpellingTagger(buffer)) as ITagger<T>;}
    internal sealed class SpellingTagger:ITagger<IErrorTag>
    {
        private readonly ITextBuffer buffer;private ITextSnapshot parsed;private string mask;private TextBox checker;private string language;
        private readonly Dictionary<string,string> cache=new Dictionary<string,string>(StringComparer.Ordinal);
        internal SpellingTagger(ITextBuffer buffer){this.buffer=buffer;buffer.Changed+=(s,e)=>{parsed=null;TagsChanged?.Invoke(this,new SnapshotSpanEventArgs(new SnapshotSpan(e.After,0,e.After.Length)));};}
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
```

### 추가 실행 대조

- 언어사전·주석/문자열·사전수명·Undo 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F075 · Underline Unknown Symbols

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- mistyped/context-invalid symbol 표시; IDE squiggle 표시가 비활성일 때만 적용

### CA의 현재 범위·차이

제한된 식별자 밑줄; 전체 언어 해석 미완료

### 현재 코드 근거 후보

 CodeAssist.Extension/SymbolTypoTagger.cs:22

```
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return buffer.Properties.GetOrCreateSingletonProperty(
                () => new SymbolTypoTagger(buffer, IndexService)) as ITagger<T>;
        }
    }
```

### 추가 실행 대조

- IDE표시 토글·정의중·include/macro·취소 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F076 · Code Inspection Settings

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-inspection/introduction-to-code-inspection)

- 개별 검사 활성·quickfix 조정·level1~5 설정; level은 UI 색이며5는 dim 처리

- editor에서 개별 검사를 enable/disable할 수 없고 tool window에서는 disable만 가능

### CA의 현재 범위·차이

끄기·파일·프로젝트 검사; 전체 checker set 미완료

### 현재 코드 근거 후보

 CodeAssist.Extension/ProductOptions.cs:279

```
            Page(editor,"Editor","Underlining",()=> {
                Toggle(UiText.Get("파일 내 기본 코드 진단 사용", StartupUi.Preferences.UiLanguage),draft.InspectionMode!=InspectionMode.Disabled,v=>draft.InspectionMode=v?InspectionMode.PerFile:InspectionMode.Disabled);
                content.Children.Add(ProductTheme.Text(UiText.Get("확정 구문·스코프 오류 후보를 표시합니다. 컴파일러 전체 진단과 동일하지 않습니다.", StartupUi.Preferences.UiLanguage)));
            });
            Page(editor,"Editor","Highlighting",()=>{
                Toggle("Highlight references under caret",draft.AutomaticReferenceHighlights,v=>draft.AutomaticReferenceHighlights=v);
                Toggle("Highlight current line",draft.HighlightCurrentLine,v=>draft.HighlightCurrentLine=v);
```

 CodeAssist.Extension/ProductOptions.cs:374

```
                content.Children.Add(ProductTheme.Text("Additional C/C++ file extensions"));var input=new TextBox {Text=draft.CustomFileExtensions};content.Children.Add(input);
                input.TextChanged+=(s,e)=>{try{FileExtensionSettings.Parse(input.Text);draft.CustomFileExtensions=input.Text;errors.Remove("extensions");status.Text=UiText.Get("설정은 저장할 때 적용됩니다.",draft.UiLanguage);}catch(ArgumentException error){errors.Add("extensions");status.Text=error.Message;}save.IsEnabled=errors.Count==0;};
                content.Children.Add(ProductTheme.Text("Example: .ipp;.tpp;.cuh. Changes are applied to the next index build and cache fingerprint. Language-service associations remain Visual Studio settings."));
            });
            Page(projects,"Projects","Compilation Database",()=>{
                content.Children.Add(ProductTheme.Text("Compilation database directory"));var input=new TextBox {Text=draft.CompilationDatabaseDirectory};content.Children.Add(input);
                input.TextChanged+=(s,e)=>{var value=input.Text.Trim();if(value.Length==0||System.IO.Directory.Exists(value)&&System.IO.File.Exists(System.IO.Path.Combine(value,"compile_commands.json"))){draft.CompilationDatabaseDirectory=value;errors.Remove("database");status.Text=UiText.Get("설정은 저장할 때 적용됩니다.",draft.UiLanguage);}else{errors.Add("database");status.Text="Select a directory containing compile_commands.json.";}save.IsEnabled=errors.Count==0;};
```

 CodeAssist.Extension/ProductOptions.cs:375

```
                input.TextChanged+=(s,e)=>{try{FileExtensionSettings.Parse(input.Text);draft.CustomFileExtensions=input.Text;errors.Remove("extensions");status.Text=UiText.Get("설정은 저장할 때 적용됩니다.",draft.UiLanguage);}catch(ArgumentException error){errors.Add("extensions");status.Text=error.Message;}save.IsEnabled=errors.Count==0;};
                content.Children.Add(ProductTheme.Text("Example: .ipp;.tpp;.cuh. Changes are applied to the next index build and cache fingerprint. Language-service associations remain Visual Studio settings."));
            });
            Page(projects,"Projects","Compilation Database",()=>{
                content.Children.Add(ProductTheme.Text("Compilation database directory"));var input=new TextBox {Text=draft.CompilationDatabaseDirectory};content.Children.Add(input);
                input.TextChanged+=(s,e)=>{var value=input.Text.Trim();if(value.Length==0||System.IO.Directory.Exists(value)&&System.IO.File.Exists(System.IO.Path.Combine(value,"compile_commands.json"))){draft.CompilationDatabaseDirectory=value;errors.Remove("database");status.Text=UiText.Get("설정은 저장할 때 적용됩니다.",draft.UiLanguage);}else{errors.Add("database");status.Text="Select a directory containing compile_commands.json.";}save.IsEnabled=errors.Count==0;};
                content.Children.Add(ProductTheme.Text("CMake, Ninja, Make and other build systems can export compile_commands.json. Select a target-specific directory to use its compiler, flags, target architecture and include paths. CodeAssist reads the database; it does not execute build commands."));
```

 CodeAssist.Extension/ProjectInspection.cs:13

```
{
    internal sealed class ProjectInspection : IDisposable
    {
        private readonly AsyncPackage package;
        private readonly ProjectIndexService index;
        private readonly ErrorListProvider errors;
        private CancellationTokenSource run;
```

 CodeAssist.Extension/ProjectInspection.cs:19

```
        private CancellationTokenSource run;
        internal ProjectInspection(AsyncPackage package, ProjectIndexService index)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            this.package=package;this.index=index;errors=new ErrorListProvider(package);
        }
        internal void Start()
```

 CodeAssist.Extension/ProjectInspection.cs:37

```
                    if(string.IsNullOrEmpty(root)) return;
                    var findings=await System.Threading.Tasks.Task.Run(()=>ProjectSyntaxInspection.Scan(root,token),token);
                    await package.JoinableTaskFactory.SwitchToMainThreadAsync(token);
                    errors.Tasks.Clear();
                    foreach(var finding in findings)
                    {
                        var task=new ErrorTask { Document=finding.File,Line=finding.Line-1,Column=finding.Column-1,Text="CodeAssist: "+finding.Message,Category=TaskCategory.CodeSense,ErrorCategory=TaskErrorCategory.Warning };
```

### 추가 실행 대조

- checker/level·설정 reset·조정·source suppression

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F077 · Code Issues in the Editor

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-inspection/introduction-to-code-inspection)

- solid 또는 dim은 fix 가능, dotted는 불가; underline과 scrollbar 위치 표시 별도

- scrollbar의 중복 issue 세부정보는 중복 제거; 표시 비활성이어도 결과 창에서 접근

### CA의 현재 범위·차이

기본 구문·스코프 후보 진단; 원본 검사 엔진 미완료

### 현재 코드 근거 후보

 CodeAssist.Extension/SymbolTypoTagger.cs:130

```
            }
            foreach (var issue in CppDelimiterParser.FindDefiniteIssues(snapshot.GetText()))
            {
                var span = new SnapshotSpan(snapshot, issue.Start, issue.Length);
                tags.Add(new TagSpan<IErrorTag>(span,
                    new ErrorTag(PredefinedErrorTypeNames.SyntaxError, issue.Message + " (CodeAssist)")));
            }
```

 CodeAssist.Extension/ProjectInspection.cs:19

```
        private CancellationTokenSource run;
        internal ProjectInspection(AsyncPackage package, ProjectIndexService index)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            this.package=package;this.index=index;errors=new ErrorListProvider(package);
        }
        internal void Start()
```

### 추가 실행 대조

- 표시 토글·겹친 issue·level5·tracking

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F078 · Quick Fixes in the Editor

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-inspection/introduction-to-code-inspection)

- hover tomato 또는 issue 앞부분 caret에서 Shift+Alt+Q로 fix 접근

- 그룹 fix는 editor Undo 한 번으로 복원; 파서는 다른 VA 파서와 독립

### CA의 현재 범위·차이

커서·선택 범위의 clangd quickfix 선택과 미리보기; 자체 전구 UI 미구현; VA 동등성 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CompilerActions.cs:3

```
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
```

 CodeAssist.Extension/CompilerActions.cs:20

```
        private sealed class ActionItem {internal JObject Action;public override string ToString()=>(string)Action["title"];}
        internal static async Task<bool> TryAsync(AsyncPackage package,EnvDTE80.DTE2 dte,bool quickFix=false,bool toolWindow=false)
        {
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();
            var file=dte.ActiveDocument?.FullName;if(string.IsNullOrEmpty(file))return false;
            var root=Path.GetDirectoryName(file);
            while(root!=null&&!File.Exists(Path.Combine(root,"compile_commands.json"))&&!File.Exists(Path.Combine(root,"compile_flags.txt")))root=Path.GetDirectoryName(root);
```

 CodeAssist.Extension/CompilerActions.cs:31

```
            if(manager==null||components==null||manager.GetActiveView(1,null,out var adapter)<0)return false;
            var view=components.GetService<IVsEditorAdaptersFactoryService>().GetWpfTextView(adapter);
            if(view==null||view.IsClosed||!view.TextBuffer.ContentType.IsOfType("C/C++"))return false;
            var snapshot=view.TextSnapshot;var span=view.Selection.StreamSelectionSpan.SnapshotSpan;
            var first=snapshot.GetLineFromPosition(span.Start);var last=snapshot.GetLineFromPosition(span.End);
            var range=new JObject {["start"]=new JObject {["line"]=first.LineNumber,["character"]=span.Start.Position-first.Start.Position},["end"]=new JObject {["line"]=last.LineNumber,["character"]=span.End.Position-last.Start.Position}};
            var open=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
```

### 추가 실행 대조

- 다중 fix·겹침·stale span·취소·Undo

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F079 · Quick Fixes in a Tool Window

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-inspection/introduction-to-code-inspection)

- 현재 파일 issue 기본 line 정렬; 단일/복수 선택과 checker/level 그룹 fix

- tracking·열 정렬·double click 이동; fix Undo는 결과 창이 아니라 editor에서 수행

### CA의 현재 범위·차이

도구 창에서 clangd quickfix 선택·미리보기·적용; 프로젝트 전체 수정 목록 아님; VA 동등성 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CompilerActions.cs:13

```
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
namespace CodeAssist.Extension
{
    internal static class CompilerActions
    {
```

 CodeAssist.Extension/CompilerActions.cs:14

```
using System.Windows;
using System.Windows.Controls;
namespace CodeAssist.Extension
{
    internal static class CompilerActions
    {
        private sealed class ActionItem {internal JObject Action;public override string ToString()=>(string)Action["title"];}
```

 CodeAssist.Extension/CompilerActions.cs:20

```
        private sealed class ActionItem {internal JObject Action;public override string ToString()=>(string)Action["title"];}
        internal static async Task<bool> TryAsync(AsyncPackage package,EnvDTE80.DTE2 dte,bool quickFix=false,bool toolWindow=false)
        {
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();
            var file=dte.ActiveDocument?.FullName;if(string.IsNullOrEmpty(file))return false;
            var root=Path.GetDirectoryName(file);
            while(root!=null&&!File.Exists(Path.Combine(root,"compile_commands.json"))&&!File.Exists(Path.Combine(root,"compile_flags.txt")))root=Path.GetDirectoryName(root);
```

### 추가 실행 대조

- 그룹/복수 선택·sort·tracking·Undo

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F080 · VA Snippets

실행 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-snippets/introduction-to-va-snippets)

- shortcut·title-only·surround·smart suggestion·refactoring 유형을 공유할 수 있음

- 편집 즉시 반영·IDE 간 사용자 snippet 공유·reserved 값과 caret 위치 확장

### CA의 현재 범위·차이

기존 장부에 구체적인 구현 범위 설명 누락. 후보 파일 존재만 확인함.

### 현재 코드 근거 후보

 CodeAssist.Extension/SnippetInsertion.cs:40

```
            {
                var path=Path.Combine(root,".codeassist","snippets.tpl");
                try {var info=new FileInfo(path);if(info.Exists&&info.Length<=1024*1024){if(!cachedLibraries.TryGetValue(path,out var cached)||cached.Item1!=info.LastWriteTimeUtc){cached=Tuple.Create(info.LastWriteTimeUtc,SnippetTemplate.Parse(File.ReadAllText(path)).ToArray());if(cachedLibraries.Count>=32)cachedLibraries.Clear();cachedLibraries[path]=cached;}templates=templates.Concat(cached.Item2).ToArray();}}
                catch(IOException){}catch(UnauthorizedAccessException){}
            }
            return templates.Where(t=>!string.IsNullOrEmpty(prefix)&&t.Shortcut.StartsWith(prefix,StringComparison.OrdinalIgnoreCase)).GroupBy(t=>t.Shortcut,StringComparer.Ordinal).Select(g=>g.Last()).Take(30).ToArray();
        }
```

 CodeAssist.Extension/SnippetInsertion.cs:55

```
            ThreadHelper.ThrowIfNotOnUIThread();
            if(!StartupUi.Preferences.SnippetShortcutsEnabled||!view.Selection.IsEmpty)return false;
            var point=view.Caret.Position.BufferPosition;var line=point.GetContainingLine();var before=line.GetText().Substring(0,point.Position-line.Start.Position);
            var match=System.Text.RegularExpressions.Regex.Match(before,@"[A-Za-z_][A-Za-z_0-9]*$");if(!match.Success)return false;
            var file=view.TextBuffer.Properties.TryGetProperty(typeof(ITextDocument),out ITextDocument document)?document.FilePath:null;
            var dte=Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
            var solution=dte?.Solution?.FullName;var root=string.IsNullOrEmpty(solution)?null:Path.GetDirectoryName(solution);
```

 CodeAssist.Extension/SnippetInsertion.cs:62

```
            var templates=new List<SnippetTemplate>(BuiltIns);
            try{var path=root==null?null:Path.Combine(root,".codeassist","snippets.tpl");if(path!=null&&File.Exists(path)&&new FileInfo(path).Length<=1024*1024)templates.AddRange(SnippetTemplate.Parse(File.ReadAllText(path)));}catch(IOException){}catch(UnauthorizedAccessException){}
            if(!templates.Any(t=>t.Shortcut==match.Value))return false;
            Show(view,file,root,dte==null?IntPtr.Zero:(IntPtr)dte.MainWindow.HWnd,null,solution,false,match.Value,new SnapshotSpan(point.Snapshot,line.Start.Position+match.Index,match.Length));return true;
        }
        internal static void Show(IWpfTextView view, string file, string projectRoot, IntPtr owner, string projectFile = null, string solutionFile = null, bool surroundOnly = false, string shortcut = null, SnapshotSpan? replacement = null)
        {
```

### 추가 실행 대조

- 유형별 메뉴·수정 반영·언어·직렬화·caret

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F081 · for Refactoring

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-snippets/va-snippets-for-refactoring-and-code-generation)

- 지정 title로 필수 snippet을 찾고 삭제 시 기본 snippet 재생성

- refactoring 전용 reserved 값이 있으며 일반 symbol-context reserved는 null 확장

### CA의 현재 범위·차이

메서드 구현 생성의 사용자 본문 템플릿과 예약 변수; 모든 리팩터링 템플릿 아님; VA 동등성 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/RefactoringCommands.cs:16

```
{
    internal static class RefactoringCommands
    {
        internal static async System.Threading.Tasks.Task InitializeAsync(AsyncPackage package)
        {
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();var service=await package.GetServiceAsync(typeof(IMenuCommandService)) as IMenuCommandService;
            if(service==null)return;
```

 CodeAssist.Extension/RefactoringCommands.cs:22

```
            if(service==null)return;
            for(var id=0x180;id<=0x18c;id++){var command=id;service.AddCommand(new OleMenuCommand((s,e)=>package.JoinableTaskFactory.RunAsync(async()=>{try{await ExecuteAsync(package,command);}catch(Exception error){await package.JoinableTaskFactory.SwitchToMainThreadAsync();MessageBox.Show(error.Message,"CodeAssist",MessageBoxButton.OK,MessageBoxImage.Warning);}}).FileAndForget("CodeAssist/Refactoring"),new CommandID(GoToSymbolCommand.CommandSet,command)));}
        }
        private static async System.Threading.Tasks.Task ExecuteAsync(AsyncPackage package,int id)
        {
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();var dte=await package.GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
            var model=await package.GetServiceAsync(typeof(SComponentModel)) as IComponentModel;var manager=await package.GetServiceAsync(typeof(SVsTextManager)) as IVsTextManager;
```

 CodeAssist.Extension/RefactoringCommands.cs:33

```
            var start=selected.Length==0?line.Start.Position:selected.Start.Position;var length=selected.Length==0?line.Length:selected.Length;
            string result=null;var title="CodeAssist Refactoring";
            string Ask(string label,string initial="")=>EditorFeatureCommands.Prompt(dte,label,initial);
            switch(id)
            {
                case 0x18b:
                    result=CppDocumentTransforms.ConvertLocalPointer(snapshot.GetText(),start,length);start=0;length=snapshot.Length;title="Convert Local Pointer / Instance";break;
```

### 추가 실행 대조

- title 변경/삭제·언어별 필수값·reserved 문맥

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F082 · for Smart Suggestions

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-snippets/va-snippets-for-smart-suggestions)

- class/loop/switch 및 타입 이름 snippet이 제안 값을 제공

- 목록은 알파벳 정렬이므로 template 내 값 순서는 영향 없음

### CA의 현재 범위·차이

기본·프로젝트 스니펫 추천과 Tab·Enter·마우스 커밋 연결; 실제 IDE 입력 검증 미완료; VA 동등성 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CodeAssistCompletionSource.cs:143

```
            if(!StartupUi.Preferences.SuggestionListsEnabled)return;
            var snippetItems=StartupUi.Preferences.SnippetSuggestionsEnabled&&!hasResolvedMemberContext&&!includeMatch.Success?SnippetInsertion.Suggestions(indexService.ProjectRoot,prefix).Select(t=>(Completion)new SnippetCompletion(t)).ToList():new List<Completion>();
            if(StartupUi.Preferences.SmartSuggestionsEnabled&&!hasResolvedMemberContext&&!includeMatch.Success)
                snippetItems.AddRange(SmartSuggestions.At(snapshot.GetText(),point.Value.Position).Where(value=>value.StartsWith(prefix,StringComparison.OrdinalIgnoreCase)).Select(value=>(Completion)new Completion(value,value,"CodeAssist expected-type literal",null,"literal")));
            if (candidates.Count == 0&&snippetItems.Count==0) return;
            var builtInSet = completionSets.FirstOrDefault(set =>
                !string.Equals(set.Moniker, "CodeAssistPreview", StringComparison.Ordinal) &&
```

 CodeAssist.Extension/CodeAssistCompletionSource.cs:144

```
            var snippetItems=StartupUi.Preferences.SnippetSuggestionsEnabled&&!hasResolvedMemberContext&&!includeMatch.Success?SnippetInsertion.Suggestions(indexService.ProjectRoot,prefix).Select(t=>(Completion)new SnippetCompletion(t)).ToList():new List<Completion>();
            if(StartupUi.Preferences.SmartSuggestionsEnabled&&!hasResolvedMemberContext&&!includeMatch.Success)
                snippetItems.AddRange(SmartSuggestions.At(snapshot.GetText(),point.Value.Position).Where(value=>value.StartsWith(prefix,StringComparison.OrdinalIgnoreCase)).Select(value=>(Completion)new Completion(value,value,"CodeAssist expected-type literal",null,"literal")));
            if (candidates.Count == 0&&snippetItems.Count==0) return;
            var builtInSet = completionSets.FirstOrDefault(set =>
                !string.Equals(set.Moniker, "CodeAssistPreview", StringComparison.Ordinal) &&
                set.Completions != null && set.Completions.Count > 0);
```

 CodeAssist.Extension/CodeAssistCompletionSource.cs:145

```
            if(StartupUi.Preferences.SmartSuggestionsEnabled&&!hasResolvedMemberContext&&!includeMatch.Success)
                snippetItems.AddRange(SmartSuggestions.At(snapshot.GetText(),point.Value.Position).Where(value=>value.StartsWith(prefix,StringComparison.OrdinalIgnoreCase)).Select(value=>(Completion)new Completion(value,value,"CodeAssist expected-type literal",null,"literal")));
            if (candidates.Count == 0&&snippetItems.Count==0) return;
            var builtInSet = completionSets.FirstOrDefault(set =>
                !string.Equals(set.Moniker, "CodeAssistPreview", StringComparison.Ordinal) &&
                set.Completions != null && set.Completions.Count > 0);
            var start = point.Value.Position - prefix.Length;
```

### 추가 실행 대조

- 타입/typedef·빈 제안·삭제와 재생성·정렬

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F083 · for Surround With

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-snippets/va-snippets-for-surround-with)

- selected reserved 값은 현재 선택 또는 빈 문자열; 같은 surround 문자가 복수면 메뉴

- surround 문자 실행은 관련 옵션 필요; title accelerator 및 메뉴/shortcut 겸용

### CA의 현재 범위·차이

selected 템플릿만 필터해 감싸기; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/SnippetInsertion.cs:66

```
        }
        internal static void Show(IWpfTextView view, string file, string projectRoot, IntPtr owner, string projectFile = null, string solutionFile = null, bool surroundOnly = false, string shortcut = null, SnapshotSpan? replacement = null)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var snapshot = view.TextSnapshot;
            var selection = replacement ?? view.Selection.StreamSelectionSpan.SnapshotSpan;
            var templates = new List<SnippetTemplate>(BuiltIns);
```

 CodeAssist.Extension/SnippetInsertion.cs:82

```
            if(shortcut!=null)templates=templates.Where(t=>string.Equals(t.Shortcut,shortcut,StringComparison.Ordinal)).ToList();
            if(surroundOnly)templates=templates.Where(t=>t.Variables().Contains("selected")).ToList();
            if(templates.Count==0)return;
            CaptureEnvironment();
            templates = templates.Select(template => template.WithEnvironment(name => environment.TryGetValue(name, out var value) ? value : "")).ToList();
            var reserved = SnippetReservedValues.Create(file, shortcut==null?selection.GetText():"", DateTime.Now, System.Globalization.CultureInfo.CurrentCulture);
            SnippetReservedValues.AddWorkspace(reserved, projectFile, solutionFile, Guid.NewGuid(), System.Globalization.CultureInfo.CurrentCulture);
```

### 추가 실행 대조

- 선택 없음·부분/전체 줄·문자 충돌·accelerator

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F084 · with Shortcuts

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-snippets/va-snippets-with-shortcuts)

- shortcut은 case-sensitive; 중복 shortcut은 모두 표시하지만 동일 shortcut-title 쌍은 하나만 표시

- 수락하면 shortcut을 expanded 코드로 교체; 제목 없으면 코드 앞부분 표시

### CA의 현재 범위·차이

단축어+Tab 확장; 추천 목록 활성 중에는 기존 처리 우선; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/SnippetInsertion.cs:55

```
            ThreadHelper.ThrowIfNotOnUIThread();
            if(!StartupUi.Preferences.SnippetShortcutsEnabled||!view.Selection.IsEmpty)return false;
            var point=view.Caret.Position.BufferPosition;var line=point.GetContainingLine();var before=line.GetText().Substring(0,point.Position-line.Start.Position);
            var match=System.Text.RegularExpressions.Regex.Match(before,@"[A-Za-z_][A-Za-z_0-9]*$");if(!match.Success)return false;
            var file=view.TextBuffer.Properties.TryGetProperty(typeof(ITextDocument),out ITextDocument document)?document.FilePath:null;
            var dte=Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
            var solution=dte?.Solution?.FullName;var root=string.IsNullOrEmpty(solution)?null:Path.GetDirectoryName(solution);
```

### 추가 실행 대조

- case·중복 쌍·language·commit·Undo

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F085 · Create from Selection

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-snippets/va-snippets-create-from-selection)

- editor 선택으로 snippet editor를 열고 title/shortcut/description 지정

- input placeholder와 end reserved를 편집하여 입력 dialog와 최종 caret 구성

### CA의 현재 범위·차이

선택 영역 스니펫 저장; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/EditorFeatureCommands.cs:55

```
            var view=components.GetService<IVsEditorAdaptersFactoryService>().GetWpfTextView(adapter);if(view==null||view.IsClosed)return;
            var snapshot=view.TextSnapshot;var selected=view.Selection.StreamSelectionSpan.SnapshotSpan;var caret=view.Caret.Position.BufferPosition.Position;var text=snapshot.GetText();
            var line=snapshot.GetLineFromPosition(caret);var word=IdentifierParser.At(line.GetText(),caret-line.Start.Position);
            if(id==0x17b)
            {
                var links=SourceLinks.Find(line.GetText(),StartupUi.Preferences.SourceLinkRules).Where(candidate=>caret-line.Start.Position>=candidate.Start&&caret-line.Start.Position<=candidate.Start+candidate.Length).ToArray();
                if(links.Length==0)throw new InvalidOperationException(UiText.Get("No source link matches the caret. Configure Navigation / Source Links.",StartupUi.Preferences.UiLanguage));
```

 CodeAssist.Extension/EditorFeatureCommands.cs:70

```
                var picker=new NavigationPicker("CodeAssist: Go To Member",entries,package.JoinableTaskFactory);new System.Windows.Interop.WindowInteropHelper(picker).Owner=(IntPtr)dte.MainWindow.HWnd;
                if(picker.ShowDialog()==true&&picker.Selected!=null){dte.ItemOperations.OpenFile(picker.Selected.File,EnvDTE.Constants.vsViewKindTextView);(dte.ActiveDocument.Selection as EnvDTE.TextSelection)?.MoveToLineAndOffset(picker.Selected.Line,picker.Selected.Column,false);}return;
            }
            if(id==0x175){if(selected.Length==0)return;SnippetInsertion.Show(view,file,indexes.ProjectRoot,(IntPtr)dte.MainWindow.HWnd,null,dte.Solution?.FullName,true);return;}
            if(id==0x176)
            {
                if(selected.Length==0)return;
```

 CodeAssist.Extension/EditorFeatureCommands.cs:76

```
                if(selected.Length==0)return;
                var title=Prompt(dte,"Snippet title","Selection");if(title==null)return;
                var shortcut=Prompt(dte,"Snippet shortcut","snippet");if(shortcut==null)return;
                var root=!string.IsNullOrEmpty(dte.Solution?.FullName)?Path.GetDirectoryName(dte.Solution.FullName):indexes.ProjectRoot;
                if(string.IsNullOrEmpty(root))throw new InvalidOperationException("Open a solution before saving a project snippet.");
                var path=Path.Combine(root,".codeassist","snippets.tpl");var existing=File.Exists(path)?File.ReadAllText(path):"";
                if(existing.Length>1024*1024)throw new InvalidOperationException("Snippet library exceeds 1 MiB.");
```

### 추가 실행 대조

- 여러 줄·공백·문자 escape·입력·caret

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F086 · Environment Placeholders

실행 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-snippets/va-snippet-environment-placeholders)

- IDE 환경 snapshot

- case-insensitive 이름·미정의 빈 값·입력보다 먼저 확장

### CA의 현재 범위·차이

기존 장부에 구체적인 구현 범위 설명 누락. 후보 파일 존재만 확인함.

### 현재 코드 근거 후보

 CodeAssist.Core/SnippetReservedValues.cs:10

```
    {
        public static Dictionary<string, string> Create(string file, string selected, DateTime now, CultureInfo culture)
        {
            culture = culture ?? CultureInfo.CurrentCulture;
            var english = CultureInfo.GetCultureInfo("en-US");
            var values = new Dictionary<string, string>(StringComparer.Ordinal)
            {
```

### 추가 실행 대조

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F087 · Input Placeholders

실행 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-snippets/va-snippet-input-placeholders)

- 기본값·반복 값 공유·case-sensitive 이름/수정자·최대8입력

- 인라인 입력과 문맥에 따른 대화상자 전환

### CA의 현재 범위·차이

기존 장부에 구체적인 구현 범위 설명 누락. 후보 파일 존재만 확인함.

### 현재 코드 근거 후보

 CodeAssist.Core/SnippetTemplate.cs:56

```
        }
        public SnippetInput[] Inputs(IReadOnlyDictionary<string, string> reserved = null)
        {
            var fields = new Dictionary<string, SnippetInput>(StringComparer.Ordinal);
            var names = new List<string>();
            foreach (var token in Tokens())
            {
```

 CodeAssist.Core/SnippetTemplate.cs:58

```
        {
            var fields = new Dictionary<string, SnippetInput>(StringComparer.Ordinal);
            var names = new List<string>();
            foreach (var token in Tokens())
            {
                var name = BaseName(token, out _, out var initial);
                if (name.Length == 0 || name == "end" || reserved != null && (reserved.ContainsKey(token) || reserved.ContainsKey(name))) continue;
```

 CodeAssist.Core/SnippetTemplate.cs:65

```
                if (!fields.TryGetValue(name, out var field))
                { fields[name] = new SnippetInput(name, initial); names.Add(name); }
                else if (field.DefaultValue == null && initial != null) fields[name] = new SnippetInput(name, initial);
            }
            return names.Select(name => fields[name]).ToArray();
        }
        public string[] Variables() => Inputs().Select(field => field.Name).ToArray();
```

### 추가 실행 대조

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F088 · Reserved Strings

실행 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/code-snippets/va-snippet-reserved-strings)

- 날짜·파일·프로젝트·솔루션·선택·클립보드·GUID·심볼 문맥·refactor 예약 값

### CA의 현재 범위·차이

기존 장부에 구체적인 구현 범위 설명 누락. 후보 파일 존재만 확인함.

### 현재 코드 근거 후보

 CodeAssist.Core/SnippetReservedValues.cs:8

```
{
    public static class SnippetReservedValues
    {
        public static Dictionary<string, string> Create(string file, string selected, DateTime now, CultureInfo culture)
        {
            culture = culture ?? CultureInfo.CurrentCulture;
            var english = CultureInfo.GetCultureInfo("en-US");
```

### 추가 실행 대조

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F089 · Address Resolver

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/debug-assistance/address-resolver)

- EXE/DLL/PDB 여러 모듈의 주소를 symbol로 해석; 올바른 실행 base 필요

- PDB 없이 exports 모드·절대/relative 주소 형식; 미해결 주소는???·비주소 텍스트는 유지

- symbol server는 VS 설정 사용; 주소에서 symbol 단방향 해석

### CA의 현재 범위·차이

LLVM symbolizer를 이용한 바이너리 RVA 해석; 절대 주소·런타임 모듈 자동 매핑 없음; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/UtilityCommands.cs:37

```
            {
                var picker=new Microsoft.Win32.OpenFileDialog {Title=id==0x190?"CodeAssist: PDB Explorer":"CodeAssist: Address Resolver",Filter=id==0x190?"PDB files|*.pdb":"Binary images|*.exe;*.dll|All files|*.*"};if(picker.ShowDialog()!=true)return;
                var tool=id==0x190?"llvm-pdbutil.exe":"llvm-symbolizer.exe";var executable=Path.GetFullPath(Path.Combine(Path.GetDirectoryName(dte.FullName),"../../VC/Tools/Llvm/x64/bin",tool));
                if(!File.Exists(executable))throw new FileNotFoundException("LLVM tool not found. Install the Visual Studio C++ LLVM tools.",executable);
                var arguments=id==0x190?"dump --publics --globals "+Quote(picker.FileName):"--obj="+Quote(picker.FileName)+" --relative-address --functions --inlines ";
                if(id==0x191){var address=EditorFeatureCommands.Prompt(dte,"Relative virtual address (RVA), hexadecimal","0x1000");if(address==null)return;if(!System.Text.RegularExpressions.Regex.IsMatch(address,@"^(?:0[xX])?[0-9a-fA-F]{1,16}$"))throw new ArgumentException("Invalid RVA");arguments+=(address.StartsWith("0x",StringComparison.OrdinalIgnoreCase)?address:"0x"+address);}
                var output=await RunToolAsync(executable,arguments,package.DisposalToken);await ShowAsync(package,Path.GetFileName(picker.FileName),new TextBox {Text=output,IsReadOnly=true,AcceptsReturn=true,FontFamily=new FontFamily("Consolas"),HorizontalScrollBarVisibility=ScrollBarVisibility.Auto,VerticalScrollBarVisibility=ScrollBarVisibility.Auto});return;
```

 CodeAssist.Extension/UtilityCommands.cs:40

```
                if(!File.Exists(executable))throw new FileNotFoundException("LLVM tool not found. Install the Visual Studio C++ LLVM tools.",executable);
                var arguments=id==0x190?"dump --publics --globals "+Quote(picker.FileName):"--obj="+Quote(picker.FileName)+" --relative-address --functions --inlines ";
                if(id==0x191){var address=EditorFeatureCommands.Prompt(dte,"Relative virtual address (RVA), hexadecimal","0x1000");if(address==null)return;if(!System.Text.RegularExpressions.Regex.IsMatch(address,@"^(?:0[xX])?[0-9a-fA-F]{1,16}$"))throw new ArgumentException("Invalid RVA");arguments+=(address.StartsWith("0x",StringComparison.OrdinalIgnoreCase)?address:"0x"+address);}
                var output=await RunToolAsync(executable,arguments,package.DisposalToken);await ShowAsync(package,Path.GetFileName(picker.FileName),new TextBox {Text=output,IsReadOnly=true,AcceptsReturn=true,FontFamily=new FontFamily("Consolas"),HorizontalScrollBarVisibility=ScrollBarVisibility.Auto,VerticalScrollBarVisibility=ScrollBarVisibility.Auto});return;
            }
            if(id==0x192)
            {
```

 CodeAssist.Extension/UtilityCommands.cs:41

```
                var arguments=id==0x190?"dump --publics --globals "+Quote(picker.FileName):"--obj="+Quote(picker.FileName)+" --relative-address --functions --inlines ";
                if(id==0x191){var address=EditorFeatureCommands.Prompt(dte,"Relative virtual address (RVA), hexadecimal","0x1000");if(address==null)return;if(!System.Text.RegularExpressions.Regex.IsMatch(address,@"^(?:0[xX])?[0-9a-fA-F]{1,16}$"))throw new ArgumentException("Invalid RVA");arguments+=(address.StartsWith("0x",StringComparison.OrdinalIgnoreCase)?address:"0x"+address);}
                var output=await RunToolAsync(executable,arguments,package.DisposalToken);await ShowAsync(package,Path.GetFileName(picker.FileName),new TextBox {Text=output,IsReadOnly=true,AcceptsReturn=true,FontFamily=new FontFamily("Consolas"),HorizontalScrollBarVisibility=ScrollBarVisibility.Auto,VerticalScrollBarVisibility=ScrollBarVisibility.Auto});return;
            }
            if(id==0x192)
            {
                if(dte.Debugger.CurrentMode!=EnvDTE.dbgDebugMode.dbgBreakMode)throw new InvalidOperationException("Pause at a breakpoint before reading memory.");
```

### 추가 실행 대조

- base/ASLR·PDB 불일치·여러 모듈·입력 형식·실패

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F090 · PDB Explorer

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/debug-assistance/pdb-explorer)

- 한 모듈의 import/export 및 release PDB symbol 탐색; running process 없이도 가능

- debugger 이동에는 base 필요·종류/unit/zero/duplicate address 필터·열 정렬

### CA의 현재 범위·차이

LLVM pdbutil publics/globals 결과 열람; 형식 트리·전체 PDB 옵션 없음; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/UtilityCommands.cs:37

```
            {
                var picker=new Microsoft.Win32.OpenFileDialog {Title=id==0x190?"CodeAssist: PDB Explorer":"CodeAssist: Address Resolver",Filter=id==0x190?"PDB files|*.pdb":"Binary images|*.exe;*.dll|All files|*.*"};if(picker.ShowDialog()!=true)return;
                var tool=id==0x190?"llvm-pdbutil.exe":"llvm-symbolizer.exe";var executable=Path.GetFullPath(Path.Combine(Path.GetDirectoryName(dte.FullName),"../../VC/Tools/Llvm/x64/bin",tool));
                if(!File.Exists(executable))throw new FileNotFoundException("LLVM tool not found. Install the Visual Studio C++ LLVM tools.",executable);
                var arguments=id==0x190?"dump --publics --globals "+Quote(picker.FileName):"--obj="+Quote(picker.FileName)+" --relative-address --functions --inlines ";
                if(id==0x191){var address=EditorFeatureCommands.Prompt(dte,"Relative virtual address (RVA), hexadecimal","0x1000");if(address==null)return;if(!System.Text.RegularExpressions.Regex.IsMatch(address,@"^(?:0[xX])?[0-9a-fA-F]{1,16}$"))throw new ArgumentException("Invalid RVA");arguments+=(address.StartsWith("0x",StringComparison.OrdinalIgnoreCase)?address:"0x"+address);}
                var output=await RunToolAsync(executable,arguments,package.DisposalToken);await ShowAsync(package,Path.GetFileName(picker.FileName),new TextBox {Text=output,IsReadOnly=true,AcceptsReturn=true,FontFamily=new FontFamily("Consolas"),HorizontalScrollBarVisibility=ScrollBarVisibility.Auto,VerticalScrollBarVisibility=ScrollBarVisibility.Auto});return;
```

### 추가 실행 대조

- release/debug PDB·base·중복주소·module refresh·disassembly 이동

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F091 · VA Memory View

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- debug session 시작 전에 integration 활성 필요; 완전한 분리는 IDE 재시작 필요

### CA의 현재 범위·차이

중단 모드에서 명시 주소의 64바이트 디버거 표현식 읽기; 라이브 실행 미검증; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/UtilityCommands.cs:46

```
            {
                if(dte.Debugger.CurrentMode!=EnvDTE.dbgDebugMode.dbgBreakMode)throw new InvalidOperationException("Pause at a breakpoint before reading memory.");
                var address=EditorFeatureCommands.Prompt(dte,"Memory address (hexadecimal), reads 64 bytes","0x0");if(address==null)return;if(!System.Text.RegularExpressions.Regex.IsMatch(address,@"^0[xX][0-9a-fA-F]{1,16}$"))throw new ArgumentException("Invalid address");
                var value=dte.Debugger.GetExpression("*(unsigned char (*)[64])"+address,false,1000);if(value==null||!value.IsValidValue)throw new InvalidOperationException(value?.Value??"Memory is unavailable");
                var text=new StringBuilder(address+Environment.NewLine+value.Value+Environment.NewLine);foreach(EnvDTE.Expression child in value.DataMembers)text.AppendLine(child.Name+" : "+child.Value);
                await ShowAsync(package,"Memory "+address,new TextBox {Text=text.ToString(),IsReadOnly=true,FontFamily=new FontFamily("Consolas"),VerticalScrollBarVisibility=ScrollBarVisibility.Auto});return;
            }
```

 CodeAssist.Extension/UtilityCommands.cs:47

```
                if(dte.Debugger.CurrentMode!=EnvDTE.dbgDebugMode.dbgBreakMode)throw new InvalidOperationException("Pause at a breakpoint before reading memory.");
                var address=EditorFeatureCommands.Prompt(dte,"Memory address (hexadecimal), reads 64 bytes","0x0");if(address==null)return;if(!System.Text.RegularExpressions.Regex.IsMatch(address,@"^0[xX][0-9a-fA-F]{1,16}$"))throw new ArgumentException("Invalid address");
                var value=dte.Debugger.GetExpression("*(unsigned char (*)[64])"+address,false,1000);if(value==null||!value.IsValidValue)throw new InvalidOperationException(value?.Value??"Memory is unavailable");
                var text=new StringBuilder(address+Environment.NewLine+value.Value+Environment.NewLine);foreach(EnvDTE.Expression child in value.DataMembers)text.AppendLine(child.Name+" : "+child.Value);
                await ShowAsync(package,"Memory "+address,new TextBox {Text=text.ToString(),IsReadOnly=true,FontFamily=new FontFamily("Consolas"),VerticalScrollBarVisibility=ScrollBarVisibility.Auto});return;
            }
            if(id==0x193)
```

 CodeAssist.Extension/UtilityCommands.cs:48

```
                var address=EditorFeatureCommands.Prompt(dte,"Memory address (hexadecimal), reads 64 bytes","0x0");if(address==null)return;if(!System.Text.RegularExpressions.Regex.IsMatch(address,@"^0[xX][0-9a-fA-F]{1,16}$"))throw new ArgumentException("Invalid address");
                var value=dte.Debugger.GetExpression("*(unsigned char (*)[64])"+address,false,1000);if(value==null||!value.IsValidValue)throw new InvalidOperationException(value?.Value??"Memory is unavailable");
                var text=new StringBuilder(address+Environment.NewLine+value.Value+Environment.NewLine);foreach(EnvDTE.Expression child in value.DataMembers)text.AppendLine(child.Name+" : "+child.Value);
                await ShowAsync(package,"Memory "+address,new TextBox {Text=text.ToString(),IsReadOnly=true,FontFamily=new FontFamily("Consolas"),VerticalScrollBarVisibility=ScrollBarVisibility.Auto});return;
            }
            if(id==0x193)
            {
```

### 추가 실행 대조

- 세션전/중 토글·재시작·debugger state 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F092 · VA Step Filter

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- integration 활성 시 창 숨김 상태에서도 active filter 적용

- solution filter는 APPDATA 또는 .va/shared; global/builtin 상태는 APPDATA

- 공유 위치에 신규 solution filter 저장하려면 .vadbg 쓰기 권한 필요

### CA의 현재 범위·차이

함수 wildcard 기반 step 이벤트의 최대 20회 step-out; 중단점 이벤트 제외, 실제 디버깅 검증 미완료; VA 동등성 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/ToolbarCommands.cs:22

```
        private readonly ReversibleOverrides<EnvDTE80.Breakpoint2,string> threads=new ReversibleOverrides<EnvDTE80.Breakpoint2,string>();
        private string spellCommand;private int filteredSteps;private DateTime lastFilteredStep;
        private EnvDTE.DebuggerEvents debuggerEvents;
        private ToolbarCommands(AsyncPackage package,EnvDTE80.DTE2 dte){this.package=package;this.dte=dte;}
        internal static async System.Threading.Tasks.Task InitializeAsync(AsyncPackage package)
        {
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();
```

 CodeAssist.Extension/ToolbarCommands.cs:124

```
                {
                    if(threads.Active)threads.Restore(ReadFilter,WriteFilter);
                    else {
                        if(dte.Debugger.CurrentMode!=EnvDTE.dbgDebugMode.dbgBreakMode||dte.Debugger.CurrentThread==null)return;
                        var thread=dte.Debugger.CurrentThread.ID.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        threads.Apply(dte.Debugger.Breakpoints.Cast<EnvDTE.Breakpoint>().OfType<EnvDTE80.Breakpoint2>().ToArray(),ReadFilter,WriteFilter,
                            filter=>string.IsNullOrWhiteSpace(filter)?"ThreadId = "+thread:"("+filter+") & (ThreadId = "+thread+")");
```

 CodeAssist.Extension/ToolbarCommands.cs:128

```
                        var thread=dte.Debugger.CurrentThread.ID.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        threads.Apply(dte.Debugger.Breakpoints.Cast<EnvDTE.Breakpoint>().OfType<EnvDTE80.Breakpoint2>().ToArray(),ReadFilter,WriteFilter,
                            filter=>string.IsNullOrWhiteSpace(filter)?"ThreadId = "+thread:"("+filter+") & (ThreadId = "+thread+")");
                    }
                    WelcomeDocumentWindow.RecordEvent("ThreadBreakpoints="+threads.Active);return;
                }
                var manager=await package.GetServiceAsync(typeof(SVsTextManager)) as IVsTextManager;
```

### 추가 실행 대조

- global/solution 우선순위·창숨김·권한·세션수명 비교

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F093 · VA Outline

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/tool-window/va-outline)

- 현재 문서와 양방향 동기화·현재 scope bold·SAL 표시 제거

- 다중 선택·refactor 가용성·source 편집/drag/drop·comment/region 노드·hover

- AutoUpdate 꺼지면 focus 시 갱신; 자동expand는 기존노드를 자동collapse하지 않음

### CA의 현재 범위·차이

현재 파일 심볼 트리입니다. VA Outline의 전체 편집·정렬·드래그 기능 동등성은 미검증입니다.

### 현재 코드 근거 후보

 CodeAssist.Extension/SymbolToolWindows.cs:16

```
    {
        public OutlineToolWindow() : base(null) { Caption = "CodeAssist Outline"; Content = new SymbolPane(false); }
    }
    [Guid("e4c882bf-d127-4272-b93c-febd6a79f502")]
    public sealed class WorkspaceToolWindow : ToolWindowPane
    {
        public WorkspaceToolWindow() : base(null) { Caption = "CodeAssist Workspace"; Content = new SymbolPane(true); }
```

### 추가 실행 대조

- 편집/선택/Undo·comment 포함·자동갱신·다중노드

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F094 · VA View

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/tool-window/va-view)

- 파일/심볼 dropdown·MRU·hover class browser와 include hierarchy 구성

- MRU는 solution별 저장·재오픈 복원·여러 IDE는 마지막 종료가 저장

- HCB는 hover symbol 기준이며 공백/주석/unknown에서 갱신하지 않음

### CA의 현재 범위·차이

작업 영역 심볼 트리입니다. VA View의 파일·HCB 구성 전체와 같은 구현은 아닙니다.

### 현재 코드 근거 후보

 CodeAssist.Extension/SymbolToolWindows.cs:21

```
    {
        public WorkspaceToolWindow() : base(null) { Caption = "CodeAssist Workspace"; Content = new SymbolPane(true); }
    }
    internal sealed class SymbolPane : UserControl
    {
        private readonly bool workspace;
        private readonly TreeView tree = new TreeView();
```

### 추가 실행 대조

- MRU 수명·다중 IDE·overload·include hierarchy·focus

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F095 · Simple Link Patterns

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- 설치 도움말: comment keyword는 세미콜론 분리·대소문자 무시, suffix는 단일 문자 집합

- value 양끝 공백 무시·복수단어는 quote·타입 불일치면 link 미생성

- 정적 line cache는 comment classification 범위를 검사하고 별도 plain-text 허용 gate가 있음

### CA의 현재 범위·차이

단순 소스 링크 규칙과 커서 위치 열기; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/EditorFeatureCommands.cs:59

```
            {
                var links=SourceLinks.Find(line.GetText(),StartupUi.Preferences.SourceLinkRules).Where(candidate=>caret-line.Start.Position>=candidate.Start&&caret-line.Start.Position<=candidate.Start+candidate.Length).ToArray();
                if(links.Length==0)throw new InvalidOperationException(UiText.Get("No source link matches the caret. Configure Navigation / Source Links.",StartupUi.Preferences.UiLanguage));
                var link=links[0];
                if(links.Length>1){var box=new ListBox {ItemsSource=links,SelectedIndex=0};var dialog=new Window {Title="CodeAssist: Source Links",Content=box,Width=750,Height=320,WindowStartupLocation=WindowStartupLocation.CenterOwner};new System.Windows.Interop.WindowInteropHelper(dialog).Owner=(IntPtr)dte.MainWindow.HWnd;box.MouseDoubleClick+=(s,e)=>dialog.DialogResult=true;dialog.PreviewKeyDown+=(s,e)=>{if(e.Key==System.Windows.Input.Key.Enter)dialog.DialogResult=true;if(e.Key==System.Windows.Input.Key.Escape)dialog.DialogResult=false;};if(dialog.ShowDialog()!=true)return;link=box.SelectedItem as SourceLinkMatch;if(link==null)return;}
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(link.Uri.AbsoluteUri){UseShellExecute=true});return;
            }
```

 CodeAssist.Extension/EditorFeatureCommands.cs:60

```
                var links=SourceLinks.Find(line.GetText(),StartupUi.Preferences.SourceLinkRules).Where(candidate=>caret-line.Start.Position>=candidate.Start&&caret-line.Start.Position<=candidate.Start+candidate.Length).ToArray();
                if(links.Length==0)throw new InvalidOperationException(UiText.Get("No source link matches the caret. Configure Navigation / Source Links.",StartupUi.Preferences.UiLanguage));
                var link=links[0];
                if(links.Length>1){var box=new ListBox {ItemsSource=links,SelectedIndex=0};var dialog=new Window {Title="CodeAssist: Source Links",Content=box,Width=750,Height=320,WindowStartupLocation=WindowStartupLocation.CenterOwner};new System.Windows.Interop.WindowInteropHelper(dialog).Owner=(IntPtr)dte.MainWindow.HWnd;box.MouseDoubleClick+=(s,e)=>dialog.DialogResult=true;dialog.PreviewKeyDown+=(s,e)=>{if(e.Key==System.Windows.Input.Key.Enter)dialog.DialogResult=true;if(e.Key==System.Windows.Input.Key.Escape)dialog.DialogResult=false;};if(dialog.ShowDialog()!=true)return;link=box.SelectedItem as SourceLinkMatch;if(link==null)return;}
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(link.Uri.AbsoluteUri){UseShellExecute=true});return;
            }
            if(id==0x171)
```

 CodeAssist.Extension/EditorFeatureCommands.cs:61

```
                if(links.Length==0)throw new InvalidOperationException(UiText.Get("No source link matches the caret. Configure Navigation / Source Links.",StartupUi.Preferences.UiLanguage));
                var link=links[0];
                if(links.Length>1){var box=new ListBox {ItemsSource=links,SelectedIndex=0};var dialog=new Window {Title="CodeAssist: Source Links",Content=box,Width=750,Height=320,WindowStartupLocation=WindowStartupLocation.CenterOwner};new System.Windows.Interop.WindowInteropHelper(dialog).Owner=(IntPtr)dte.MainWindow.HWnd;box.MouseDoubleClick+=(s,e)=>dialog.DialogResult=true;dialog.PreviewKeyDown+=(s,e)=>{if(e.Key==System.Windows.Input.Key.Enter)dialog.DialogResult=true;if(e.Key==System.Windows.Input.Key.Escape)dialog.DialogResult=false;};if(dialog.ShowDialog()!=true)return;link=box.SelectedItem as SourceLinkMatch;if(link==null)return;}
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(link.Uri.AbsoluteUri){UseShellExecute=true});return;
            }
            if(id==0x171)
            {
```

### 추가 실행 대조

- keyword/suffix 공백·quote·타입 불일치

- 동일 줄 edit/재배치·plain text gate·comment classification

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F096 · Complex Link Patterns

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- HwPrefs.GetRegexPattern은 RE= 접두사를 대소문자 무시로 판정하고 뒤 pattern 반환

- CreateRegex는 IgnoreCase/Singleline 사용; runtime은 Compiled 추가

- timeout 생성자를 reflection으로 시도하고 실패하면 timeout 없는 생성자로 fallback

- marker match 시간이 threshold 이상이면 해당 regex/group 슬롯 비활성화; 기본 getter fallback100ms는 실제 전체 실행시간 상한 보장 아님

### CA의 현재 범위·차이

정규식·이름 있는 캡처·타임아웃 소스 링크; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/EditorFeatureCommands.cs:59

```
            {
                var links=SourceLinks.Find(line.GetText(),StartupUi.Preferences.SourceLinkRules).Where(candidate=>caret-line.Start.Position>=candidate.Start&&caret-line.Start.Position<=candidate.Start+candidate.Length).ToArray();
                if(links.Length==0)throw new InvalidOperationException(UiText.Get("No source link matches the caret. Configure Navigation / Source Links.",StartupUi.Preferences.UiLanguage));
                var link=links[0];
                if(links.Length>1){var box=new ListBox {ItemsSource=links,SelectedIndex=0};var dialog=new Window {Title="CodeAssist: Source Links",Content=box,Width=750,Height=320,WindowStartupLocation=WindowStartupLocation.CenterOwner};new System.Windows.Interop.WindowInteropHelper(dialog).Owner=(IntPtr)dte.MainWindow.HWnd;box.MouseDoubleClick+=(s,e)=>dialog.DialogResult=true;dialog.PreviewKeyDown+=(s,e)=>{if(e.Key==System.Windows.Input.Key.Enter)dialog.DialogResult=true;if(e.Key==System.Windows.Input.Key.Escape)dialog.DialogResult=false;};if(dialog.ShowDialog()!=true)return;link=box.SelectedItem as SourceLinkMatch;if(link==null)return;}
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(link.Uri.AbsoluteUri){UseShellExecute=true});return;
            }
```

 CodeAssist.Extension/EditorFeatureCommands.cs:60

```
                var links=SourceLinks.Find(line.GetText(),StartupUi.Preferences.SourceLinkRules).Where(candidate=>caret-line.Start.Position>=candidate.Start&&caret-line.Start.Position<=candidate.Start+candidate.Length).ToArray();
                if(links.Length==0)throw new InvalidOperationException(UiText.Get("No source link matches the caret. Configure Navigation / Source Links.",StartupUi.Preferences.UiLanguage));
                var link=links[0];
                if(links.Length>1){var box=new ListBox {ItemsSource=links,SelectedIndex=0};var dialog=new Window {Title="CodeAssist: Source Links",Content=box,Width=750,Height=320,WindowStartupLocation=WindowStartupLocation.CenterOwner};new System.Windows.Interop.WindowInteropHelper(dialog).Owner=(IntPtr)dte.MainWindow.HWnd;box.MouseDoubleClick+=(s,e)=>dialog.DialogResult=true;dialog.PreviewKeyDown+=(s,e)=>{if(e.Key==System.Windows.Input.Key.Enter)dialog.DialogResult=true;if(e.Key==System.Windows.Input.Key.Escape)dialog.DialogResult=false;};if(dialog.ShowDialog()!=true)return;link=box.SelectedItem as SourceLinkMatch;if(link==null)return;}
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(link.Uri.AbsoluteUri){UseShellExecute=true});return;
            }
            if(id==0x171)
```

 CodeAssist.Extension/EditorFeatureCommands.cs:61

```
                if(links.Length==0)throw new InvalidOperationException(UiText.Get("No source link matches the caret. Configure Navigation / Source Links.",StartupUi.Preferences.UiLanguage));
                var link=links[0];
                if(links.Length>1){var box=new ListBox {ItemsSource=links,SelectedIndex=0};var dialog=new Window {Title="CodeAssist: Source Links",Content=box,Width=750,Height=320,WindowStartupLocation=WindowStartupLocation.CenterOwner};new System.Windows.Interop.WindowInteropHelper(dialog).Owner=(IntPtr)dte.MainWindow.HWnd;box.MouseDoubleClick+=(s,e)=>dialog.DialogResult=true;dialog.PreviewKeyDown+=(s,e)=>{if(e.Key==System.Windows.Input.Key.Enter)dialog.DialogResult=true;if(e.Key==System.Windows.Input.Key.Escape)dialog.DialogResult=false;};if(dialog.ShowDialog()!=true)return;link=box.SelectedItem as SourceLinkMatch;if(link==null)return;}
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(link.Uri.AbsoluteUri){UseShellExecute=true});return;
            }
            if(id==0x171)
            {
```

### 추가 실행 대조

- RE= 대소문자·빈/잘못된pattern·group

- timeout 생성 실패fallback·긴match·재활성화 수명

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F097 · Connect to FogBugz and GitHub

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/source-links/built-in-plug-ins)

- issue/case tooltip은 plugin 설정과 read 권한 필요; 열기는 별도 browser login 필요할 수 있음

- 외부 서비스 API/인증의 현재 상태와2630 호환성은 미확인

### CA의 현재 범위·차이

GitHub·FogBugz URL 규칙 예제; 서비스 API 연동 없음; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/ProductOptions.cs:347

```
                buttons.Children.Add(ProductTheme.Button("Delete",()=>{if(list.SelectedItem is SourceLinkRule rule){rules.Remove(rule);draft.SourceLinkRules=rules.ToList();}}));
                buttons.Children.Add(ProductTheme.Button("GitHub example",()=>{list.SelectedItem=null;name.Text="GitHub issues";pattern.Text="issue #{id}";target.Text="https://github.com/OWNER/REPOSITORY/issues/{id}";complex.IsChecked=false;}));
                buttons.Children.Add(ProductTheme.Button("FogBugz example",()=>{list.SelectedItem=null;name.Text="FogBugz cases";pattern.Text="case {id}";target.Text="https://YOUR-SITE.fogbugz.com/f/cases/{id}";complex.IsChecked=false;}));
                content.Children.Add(ProductTheme.Text("Simple: issue #{id}. Regex: BUG-(?<id>[0-9]+). Target: https://example.com/issues/{id}. Open a matching link with CodeAssist: Open Source Link."));
            });
            Page(navigation,"Navigation","CodeAssist Hashtags",()=>content.Children.Add(ProductTheme.Text(UiText.Get("주석의 #태그를 Alt+Shift+H로 검색합니다.", StartupUi.Preferences.UiLanguage))));
            var suggestions = Group("Suggestions",()=>{
```

 CodeAssist.Extension/ProductOptions.cs:348

```
                buttons.Children.Add(ProductTheme.Button("GitHub example",()=>{list.SelectedItem=null;name.Text="GitHub issues";pattern.Text="issue #{id}";target.Text="https://github.com/OWNER/REPOSITORY/issues/{id}";complex.IsChecked=false;}));
                buttons.Children.Add(ProductTheme.Button("FogBugz example",()=>{list.SelectedItem=null;name.Text="FogBugz cases";pattern.Text="case {id}";target.Text="https://YOUR-SITE.fogbugz.com/f/cases/{id}";complex.IsChecked=false;}));
                content.Children.Add(ProductTheme.Text("Simple: issue #{id}. Regex: BUG-(?<id>[0-9]+). Target: https://example.com/issues/{id}. Open a matching link with CodeAssist: Open Source Link."));
            });
            Page(navigation,"Navigation","CodeAssist Hashtags",()=>content.Children.Add(ProductTheme.Text(UiText.Get("주석의 #태그를 Alt+Shift+H로 검색합니다.", StartupUi.Preferences.UiLanguage))));
            var suggestions = Group("Suggestions",()=>{
                Toggle(UiText.Get("Acronym completion",draft.UiLanguage),draft.AcronymCompletionEnabled,v=>draft.AcronymCompletionEnabled=v);
```

### 추가 실행 대조

- 공유/사용자 설정·권한 실패·tooltip 취소·browser login

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F098 · Preview Files

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/source-links/built-in-plug-ins)

- File Viewer는 comment 경로의 text/HTML/XAML/RTF/image/video 등을 preview

- relative 경로는 활성문서 기준·공백 quote/%20·환경값·HTML anchor 지원

### CA의 현재 범위·차이

2 MiB 이하 파일의 읽기 전용 색상 미리보기 도구 창; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/UtilityCommands.cs:37

```
            {
                var picker=new Microsoft.Win32.OpenFileDialog {Title=id==0x190?"CodeAssist: PDB Explorer":"CodeAssist: Address Resolver",Filter=id==0x190?"PDB files|*.pdb":"Binary images|*.exe;*.dll|All files|*.*"};if(picker.ShowDialog()!=true)return;
                var tool=id==0x190?"llvm-pdbutil.exe":"llvm-symbolizer.exe";var executable=Path.GetFullPath(Path.Combine(Path.GetDirectoryName(dte.FullName),"../../VC/Tools/Llvm/x64/bin",tool));
                if(!File.Exists(executable))throw new FileNotFoundException("LLVM tool not found. Install the Visual Studio C++ LLVM tools.",executable);
                var arguments=id==0x190?"dump --publics --globals "+Quote(picker.FileName):"--obj="+Quote(picker.FileName)+" --relative-address --functions --inlines ";
                if(id==0x191){var address=EditorFeatureCommands.Prompt(dte,"Relative virtual address (RVA), hexadecimal","0x1000");if(address==null)return;if(!System.Text.RegularExpressions.Regex.IsMatch(address,@"^(?:0[xX])?[0-9a-fA-F]{1,16}$"))throw new ArgumentException("Invalid RVA");arguments+=(address.StartsWith("0x",StringComparison.OrdinalIgnoreCase)?address:"0x"+address);}
                var output=await RunToolAsync(executable,arguments,package.DisposalToken);await ShowAsync(package,Path.GetFileName(picker.FileName),new TextBox {Text=output,IsReadOnly=true,AcceptsReturn=true,FontFamily=new FontFamily("Consolas"),HorizontalScrollBarVisibility=ScrollBarVisibility.Auto,VerticalScrollBarVisibility=ScrollBarVisibility.Auto});return;
```

 CodeAssist.Extension/UtilityCommands.cs:54

```
            {
                var picker=new Microsoft.Win32.OpenFileDialog {Title="CodeAssist: Preview File",Filter="C/C++ files|*.h;*.hpp;*.cpp;*.c;*.cc;*.inl|All files|*.*"};if(picker.ShowDialog()!=true)return;
                if(new FileInfo(picker.FileName).Length>2*1024*1024)throw new InvalidOperationException("Preview is limited to 2 MiB.");var text=await System.Threading.Tasks.Task.Run(()=>File.ReadAllText(picker.FileName));await ShowAsync(package,Path.GetFileName(picker.FileName),new FlowDocumentScrollViewer {Document=ColoredDocument(text),IsToolBarVisible=true});return;
            }
            if(id==0x194)
            {
                var document=dte.ActiveDocument?.Object("TextDocument") as EnvDTE.TextDocument;if(document==null)return;var text=document.StartPoint.CreateEditPoint().GetText(document.EndPoint);
```

 CodeAssist.Extension/UtilityCommands.cs:55

```
                var picker=new Microsoft.Win32.OpenFileDialog {Title="CodeAssist: Preview File",Filter="C/C++ files|*.h;*.hpp;*.cpp;*.c;*.cc;*.inl|All files|*.*"};if(picker.ShowDialog()!=true)return;
                if(new FileInfo(picker.FileName).Length>2*1024*1024)throw new InvalidOperationException("Preview is limited to 2 MiB.");var text=await System.Threading.Tasks.Task.Run(()=>File.ReadAllText(picker.FileName));await ShowAsync(package,Path.GetFileName(picker.FileName),new FlowDocumentScrollViewer {Document=ColoredDocument(text),IsToolBarVisible=true});return;
            }
            if(id==0x194)
            {
                var document=dte.ActiveDocument?.Object("TextDocument") as EnvDTE.TextDocument;if(document==null)return;var text=document.StartPoint.CreateEditPoint().GetText(document.EndPoint);
                if(text.Length>2*1024*1024)throw new InvalidOperationException("Color printing is limited to 2 MiB.");var dialog=new PrintDialog();if(dialog.ShowDialog()==true){var documentToPrint=ColoredDocument(text);documentToPrint.PageWidth=dialog.PrintableAreaWidth;documentToPrint.ColumnWidth=dialog.PrintableAreaWidth;dialog.PrintDocument(((IDocumentPaginatorSource)documentToPrint).DocumentPaginator,"CodeAssist: "+dte.ActiveDocument.Name);}return;
```

### 추가 실행 대조

- 로컬/network·경로공백·환경값·미존재파일·형식별 preview

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F099 · Open Hyperlinks in an External Browser

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- 설치 도움말: plugin이 URL을 제공하면 behavior 설정으로 internal/external browser 결정

- plugin이 override한 preference는 read-only로 표시됨

- URL template은 Value 및 선택 Keyword placeholder를 사용; 실제 launch/escape 처리는 미확인

### CA의 현재 범위·차이

http·https 링크 외부 열기; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/EditorFeatureCommands.cs:41

```
            var file=dte.ActiveDocument?.FullName;
            if(id==0x172){var path=ChooseNewFile(file);if(path==null)return;WriteNew(path,"");dte.ItemOperations.OpenFile(path,EnvDTE.Constants.vsViewKindTextView);return;}
            if(id==0x173)
            {
                var document=dte.ActiveDocument;var item=document?.ProjectItem;
                if(document==null||item==null)throw new InvalidOperationException(UiText.Get("Select a project file first.",StartupUi.Preferences.UiLanguage));
                if(!document.Saved)throw new InvalidOperationException(UiText.Get("Save the document before renaming it.",StartupUi.Preferences.UiLanguage));
```

 CodeAssist.Extension/EditorFeatureCommands.cs:70

```
                var picker=new NavigationPicker("CodeAssist: Go To Member",entries,package.JoinableTaskFactory);new System.Windows.Interop.WindowInteropHelper(picker).Owner=(IntPtr)dte.MainWindow.HWnd;
                if(picker.ShowDialog()==true&&picker.Selected!=null){dte.ItemOperations.OpenFile(picker.Selected.File,EnvDTE.Constants.vsViewKindTextView);(dte.ActiveDocument.Selection as EnvDTE.TextSelection)?.MoveToLineAndOffset(picker.Selected.Line,picker.Selected.Column,false);}return;
            }
            if(id==0x175){if(selected.Length==0)return;SnippetInsertion.Show(view,file,indexes.ProjectRoot,(IntPtr)dte.MainWindow.HWnd,null,dte.Solution?.FullName,true);return;}
            if(id==0x176)
            {
                if(selected.Length==0)return;
```

 CodeAssist.Extension/EditorFeatureCommands.cs:79

```
                var root=!string.IsNullOrEmpty(dte.Solution?.FullName)?Path.GetDirectoryName(dte.Solution.FullName):indexes.ProjectRoot;
                if(string.IsNullOrEmpty(root))throw new InvalidOperationException("Open a solution before saving a project snippet.");
                var path=Path.Combine(root,".codeassist","snippets.tpl");var existing=File.Exists(path)?File.ReadAllText(path):"";
                if(existing.Length>1024*1024)throw new InvalidOperationException("Snippet library exceeds 1 MiB.");
                if(SnippetTemplate.Parse(existing).Any(t=>t.Shortcut==shortcut))throw new InvalidOperationException("Snippet shortcut already exists.");
                var entry=CppGeneration.SnippetFromSelection(title,shortcut,selected.GetText());Directory.CreateDirectory(Path.GetDirectoryName(path));
                var temporary=path+"."+Guid.NewGuid().ToString("N")+".tmp";
```

### 추가 실행 대조

- plugin override·internal/external·URL expansion

- 실행실패·keyboard/mouse/QuickInfo·선택 URI

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F100 · Custom Plug-ins

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

공식 문서

- REA metadata: IPluginFactory Name/CreatePlugin, IPlugin Flags/GetTooltip/GetUrl/Execute/ContextExecute/GetRegex/Configure 계약 존재

- ValidateType은 분리 AppDomain helper에서 검증하고 finally에 validation domain 참조를 해제

- 검색 경로는 known directories·AppDomain base/relative path·추가 plugin paths를 모음

- SourceLinksAST에는 automation/test helper가 포함되므로 grammar parser로 해석하지 않음

### CA의 현재 범위·차이

설치된 VSIX MEF ICodeAssistPlugin 계약·문서 편집·미리보기·Undo; 별도 플러그인 SDK 배포·호환성 검증 없음; VA 동등성 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/PluginCommands.cs:15

```
{
    internal static class PluginCommands
    {
        internal static async System.Threading.Tasks.Task InitializeAsync(AsyncPackage package)
        {
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();var commands=await package.GetServiceAsync(typeof(IMenuCommandService)) as IMenuCommandService;
            commands?.AddCommand(new OleMenuCommand((s,e)=>package.JoinableTaskFactory.RunAsync(async()=>{try{await ExecuteAsync(package);}catch(Exception error){await package.JoinableTaskFactory.SwitchToMainThreadAsync();MessageBox.Show(error.Message,"CodeAssist Plugins");}}).FileAndForget("CodeAssist/Plugins"),new CommandID(GoToSymbolCommand.CommandSet,0x1b0)));
```

 CodeAssist.Extension/PluginCommands.cs:20

```
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();var commands=await package.GetServiceAsync(typeof(IMenuCommandService)) as IMenuCommandService;
            commands?.AddCommand(new OleMenuCommand((s,e)=>package.JoinableTaskFactory.RunAsync(async()=>{try{await ExecuteAsync(package);}catch(Exception error){await package.JoinableTaskFactory.SwitchToMainThreadAsync();MessageBox.Show(error.Message,"CodeAssist Plugins");}}).FileAndForget("CodeAssist/Plugins"),new CommandID(GoToSymbolCommand.CommandSet,0x1b0)));
        }
        private static async System.Threading.Tasks.Task ExecuteAsync(AsyncPackage package)
        {
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();var model=await package.GetServiceAsync(typeof(SComponentModel)) as IComponentModel;var manager=await package.GetServiceAsync(typeof(SVsTextManager)) as IVsTextManager;var dte=await package.GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
            if(model==null||manager==null||dte==null||manager.GetActiveView(1,null,out var adapter)<0)return;var view=model.GetService<IVsEditorAdaptersFactoryService>().GetWpfTextView(adapter);if(view==null||view.IsClosed)return;
```

 CodeAssist.Extension/PluginCommands.cs:27

```
            var snapshot=view.TextSnapshot;var selection=view.Selection.StreamSelectionSpan.SnapshotSpan;var caret=view.Caret.Position.BufferPosition.Position;
            var context=new PluginDocument(dte.ActiveDocument?.FullName,snapshot.GetText(),caret,selection.Start,selection.Length);var providers=model.GetExtensions<ICodeAssistPlugin>().ToArray();
            var actions=await System.Threading.Tasks.Task.Run(()=>providers.SelectMany(provider=>provider.GetActions(context)??Enumerable.Empty<PluginAction>()).Take(1000).ToArray());
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();if(actions.Length==0){MessageBox.Show("No installed VSIX exports CodeAssist.Core.ICodeAssistPlugin actions for this document.","CodeAssist Plugins");return;}
            var list=new ListBox {ItemsSource=actions,SelectedIndex=0};var dialog=new Window {Title="CodeAssist Plugins",Content=list,Width=650,Height=400,WindowStartupLocation=WindowStartupLocation.CenterOwner};new System.Windows.Interop.WindowInteropHelper(dialog).Owner=(IntPtr)dte.MainWindow.HWnd;
            list.MouseDoubleClick+=(s,e)=>dialog.DialogResult=true;dialog.PreviewKeyDown+=(s,e)=>{if(e.Key==System.Windows.Input.Key.Enter)dialog.DialogResult=true;if(e.Key==System.Windows.Input.Key.Escape)dialog.DialogResult=false;};if(dialog.ShowDialog()!=true||!(list.SelectedItem is PluginAction action))return;
            var after=await System.Threading.Tasks.Task.Run(()=>PluginEdits.Apply(context.Text,action.Edits()));await package.JoinableTaskFactory.SwitchToMainThreadAsync();
```

### 추가 실행 대조

- 올바른/잘못된 type·의존성 누락·validation 예외

- plugin settings scope·설정변경·dispose·reload

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F101 · Context Menus

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/configuration/context-menus)

- IDE context 메뉴를 확장할 때 VA submenu 구분

- 별도 VA 메뉴는 지정 modified mouse로 호출 가능

### CA의 현재 범위·차이

실제 명령을 편집기 우클릭 메뉴에 배치; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CodeAssistCommands.vsct:8

```
  <Commands package="guidCodeAssistPackage">
    <Menus>
      <Menu guid="guidCodeAssistCommandSet" id="CodeAssistToolsMenu" priority="0x0100" type="Menu"><Parent guid="guidCodeAssistCommandSet" id="CodeAssistToolsMenuGroup" /><Strings><ButtonText>CodeAssist</ButtonText></Strings></Menu>
      <Menu guid="guidCodeAssistCommandSet" id="CodeAssistContextMenu" priority="0x0700" type="Menu"><Parent guid="guidSHLMainMenu" id="IDM_VS_CTXT_CODEWIN" /><Strings><ButtonText>CodeAssist</ButtonText></Strings></Menu>
      <Menu guid="guidCodeAssistCommandSet" id="CodeAssistToolbar" priority="0x0100" type="Toolbar">
        <CommandFlag>DefaultDocked</CommandFlag><Strings><ButtonText>CodeAssist</ButtonText><CommandName>CodeAssist</CommandName></Strings>
      </Menu>
```

 CodeAssist.Extension/CodeAssistCommands.vsct:10

```
      <Menu guid="guidCodeAssistCommandSet" id="CodeAssistToolsMenu" priority="0x0100" type="Menu"><Parent guid="guidCodeAssistCommandSet" id="CodeAssistToolsMenuGroup" /><Strings><ButtonText>CodeAssist</ButtonText></Strings></Menu>
      <Menu guid="guidCodeAssistCommandSet" id="CodeAssistContextMenu" priority="0x0700" type="Menu"><Parent guid="guidSHLMainMenu" id="IDM_VS_CTXT_CODEWIN" /><Strings><ButtonText>CodeAssist</ButtonText></Strings></Menu>
      <Menu guid="guidCodeAssistCommandSet" id="CodeAssistToolbar" priority="0x0100" type="Toolbar">
        <CommandFlag>DefaultDocked</CommandFlag><Strings><ButtonText>CodeAssist</ButtonText><CommandName>CodeAssist</CommandName></Strings>
      </Menu>
    </Menus>
    <Groups>
```

 CodeAssist.Extension/CodeAssistCommands.vsct:14

```
      </Menu>
    </Menus>
    <Groups>
      <Group guid="guidCodeAssistCommandSet" id="CodeAssistToolsMenuGroup" priority="0x0600"><Parent guid="guidSHLMainMenu" id="IDM_VS_MENU_TOOLS" /></Group>
      <Group guid="guidCodeAssistCommandSet" id="CodeAssistContextGroup" priority="0x0100"><Parent guid="guidCodeAssistCommandSet" id="CodeAssistContextMenu" /></Group>
      <Group guid="guidCodeAssistCommandSet" id="CodeAssistToolbarGroup" priority="0x0100"><Parent guid="guidCodeAssistCommandSet" id="CodeAssistToolbar" /></Group>
      <Group guid="guidCodeAssistCommandSet" id="CodeAssistCommandGroup" priority="0x0600">
```

### 추가 실행 대조

- symbol/selection/file 문맥·focus·modified mouse

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F102 · Custom File Extensions

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/configuration/custom-file-extensions)

- 무시 확장자는 세미콜론 구분·이미 열린 파일은 다시 열어 반영

- extensionless C++ header는 VA 옵션과 IDE 언어 mapping 필요; custom extension 절차는 IDE별 차이

### CA의 현재 범위·차이

사용자 확장자 인덱싱·작업자·캐시 fingerprint; VS 언어 서비스 연결은 사용자 설정; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/ProjectIndexService.cs:21

```
    {
        private string[] configuredExtensions=Array.Empty<string>();
        private readonly object gate = new object();
        private Task buildTask;
        private TaskCompletionSource<bool> projectReady;
        private readonly CancellationTokenSource shutdown = new CancellationTokenSource();
        private string initializedProjectRoot;
```

 CodeAssist.Extension/ProjectIndexService.cs:52

```
        {
            target.ParseConcurrency=StartupUi.Preferences.ParseConcurrency;target.AdditionalSourceExtensions=configuredExtensions;
            return IndexBuildRunner.Build(target, mode, root, project,
                Path.Combine(Path.GetDirectoryName(typeof(ProjectIndexService).Assembly.Location), "CodeAssist.Worker.exe"),
                Environment.GetEnvironmentVariable("CODEASSIST_USE_INDEX_WORKER") == "1", shutdown.Token);
        }
        public void Dispose() { shutdown.Cancel(); }
```

 CodeAssist.Extension/ProjectIndexService.cs:101

```
            if (string.IsNullOrWhiteSpace(unrealProjectPath)) return;
            var extensions=FileExtensionSettings.Parse(StartupUi.Preferences.CustomFileExtensions);
            var projectRoot = Path.GetDirectoryName(unrealProjectPath);
            var engineRoot = string.Equals(Path.GetExtension(unrealProjectPath),".uproject",StringComparison.OrdinalIgnoreCase) ? FindEngineRoot(unrealProjectPath) : null;
            var projectCachePath = GetProjectCachePath(projectRoot);
            var engineCachePath = GetEngineCachePath(engineRoot);
```

### 추가 실행 대조

- ignore/reopen·extensionless·custom 언어·VS버전

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F103 · Keyboard Shortcuts

실행 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/configuration/keyboard-shortcuts)

- 기본키가 비어 있을 때만 설치 배정하며 기존 배정을 덮어쓰지 않음

- 추천키로 scope 확대·충돌키 재배정; global/editor scope 구분·현재키 조회

### CA의 현재 범위·차이

기존 장부에 구체적인 구현 범위 설명 누락. 후보 파일 존재만 확인함.

### 현재 코드 근거 후보

### 추가 실행 대조

- 기존키 충돌·추천 수락·scope·locale·다중 shortcut

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F104 · Mouse

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/configuration/mouse)

- modified click은 GoTo/Related/VA 메뉴에 연결하며 editor scope

- modified wheel은 no-op 또는 selection/block 확장·축소; Ctrl+Alt+wheel zoom은 유지

### CA의 현재 범위·차이

선택 옵션 Alt+클릭 정의 이동; VA 전체 마우스 제스처 미구현; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/ReferenceHighlighting.cs:25

```
        private ITextSnapshot parsed,published;private CppReferenceContext context;private int[] offsets=Array.Empty<int>();private int length;private bool closed,running;
        internal ReferenceHighlighting(ITextView view){this.view=view;view.Caret.PositionChanged+=Caret;view.TextBuffer.Changed+=Changed;view.Closed+=Closed;timer.Tick+=Tick;StartupUi.SettingsApplied+=Schedule;if(view is IWpfTextView wpf)wpf.VisualElement.PreviewMouseLeftButtonUp+=Mouse;Schedule();}
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
        private void Caret(object sender,CaretPositionChangedEventArgs e)=>Schedule();private void Changed(object sender,TextContentChangedEventArgs e)=>Schedule();
        private void Schedule(){timer.Stop();if(!closed)timer.Start();}
        private void Tick(object sender,EventArgs e){timer.Stop();if(closed)return;if(running){Schedule();return;}ExtensionPackage.Current?.JoinableTaskFactory.RunAsync(UpdateAsync).FileAndForget("CodeAssist/ReferenceHighlighting");}
        private async System.Threading.Tasks.Task UpdateAsync()
```

 CodeAssist.Extension/ReferenceHighlighting.cs:43

```
        }
        private void Mouse(object sender,MouseButtonEventArgs e){ThreadHelper.ThrowIfNotOnUIThread();if(!closed&&StartupUi.Preferences.AltClickGoToDefinition&&Keyboard.Modifiers==ModifierKeys.Alt){e.Handled=true;StartupUi.Execute(0x100);}}
        private void Raise(){if(!closed)TagsChanged?.Invoke(this,new SnapshotSpanEventArgs(new SnapshotSpan(view.TextSnapshot,0,view.TextSnapshot.Length)));}
        public IEnumerable<ITagSpan<TextMarkerTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {if(closed||spans.Count==0||spans[0].Snapshot!=published)yield break;foreach(var offset in offsets){var span=new SnapshotSpan(published,offset,length);if(spans.Any(requested=>requested.IntersectsWith(span)))yield return new TagSpan<TextMarkerTag>(span,new TextMarkerTag(WordHighlights.Marker));}}
        private void Closed(object sender,EventArgs e){closed=true;timer.Stop();timer.Tick-=Tick;view.Caret.PositionChanged-=Caret;view.TextBuffer.Changed-=Changed;view.Closed-=Closed;StartupUi.SettingsApplied-=Schedule;if(view is IWpfTextView wpf)wpf.VisualElement.PreviewMouseLeftButtonUp-=Mouse;context=null;offsets=Array.Empty<int>();}
    }
```

 CodeAssist.Extension/ReferenceHighlighting.cs:47

```
        {if(closed||spans.Count==0||spans[0].Snapshot!=published)yield break;foreach(var offset in offsets){var span=new SnapshotSpan(published,offset,length);if(spans.Any(requested=>requested.IntersectsWith(span)))yield return new TagSpan<TextMarkerTag>(span,new TextMarkerTag(WordHighlights.Marker));}}
        private void Closed(object sender,EventArgs e){closed=true;timer.Stop();timer.Tick-=Tick;view.Caret.PositionChanged-=Caret;view.TextBuffer.Changed-=Changed;view.Closed-=Closed;StartupUi.SettingsApplied-=Schedule;if(view is IWpfTextView wpf)wpf.VisualElement.PreviewMouseLeftButtonUp-=Mouse;context=null;offsets=Array.Empty<int>();}
    }
}
```

### 추가 실행 대조

- click 조합·wheel 방향·selection·IDE 충돌

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F105 · Multiple Platforms

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/configuration/project-directories)

- 편집 platform은 build platform 변경만으로 전환되지 않으며 C++ directories에서 선택 후 재시작

- platform별 header 검색 경로와 정의를 구별해야 함

### CA의 현재 범위·차이

타깃별 compilation database 디렉터리 선택; 원격 플랫폼 디버깅 없음; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CompilerSemanticService.cs:14

```
    {
        internal static readonly CompilerSemanticService Instance = new CompilerSemanticService();
        private readonly SemaphoreSlim gate = new SemaphoreSlim(1, 1);
        private readonly CancellationTokenSource shutdown = new CancellationTokenSource();
        private ClangdClient client;
        private string currentRoot;private string currentDatabase;
        internal string LastError { get; private set; }
```

### 추가 실행 대조

- Win32/x64/custom·build/edit 차이·재시작

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F106 · Non-Microsoft Build Processes

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/configuration/project-directories)

- IDE 프로젝트 설정이 비어 있는 외부 build 환경은 Custom directories로 보완

- 검색 순서 유지·한 custom set 보존·변경 인식 Rebuild 조건; 모든 build system 자동 지원은 미확인

### CA의 현재 범위·차이

외부 빌드 시스템 compile_commands.json을 clangd에 지정; 빌드 명령은 실행 안 함; 실제 IDE 및 VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/CompilerSemanticService.cs:14

```
    {
        internal static readonly CompilerSemanticService Instance = new CompilerSemanticService();
        private readonly SemaphoreSlim gate = new SemaphoreSlim(1, 1);
        private readonly CancellationTokenSource shutdown = new CancellationTokenSource();
        private ClangdClient client;
        private string currentRoot;private string currentDatabase;
        internal string LastError { get; private set; }
```

### 추가 실행 대조

- solution없음·custom 경로순서·환경값·Rebuild

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F107 · Parse Concurrency

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/configuration/parse-concurrency)

- 현재 공식 문서 기본은 모든 CPU/core; single 또는 maximum threads 제한 가능

- thread 제한 변경은 IDE 재시작; 검사 전용 max1/queue4와 일반 파서 설정은 별개

### CA의 현재 범위·차이

파서 스레드 수 설정과 작업자 전달; 실제 IDE·VA 비교 미검증

### 현재 코드 근거 후보

 CodeAssist.Extension/ProjectIndexService.cs:52

```
        {
            target.ParseConcurrency=StartupUi.Preferences.ParseConcurrency;target.AdditionalSourceExtensions=configuredExtensions;
            return IndexBuildRunner.Build(target, mode, root, project,
                Path.Combine(Path.GetDirectoryName(typeof(ProjectIndexService).Assembly.Location), "CodeAssist.Worker.exe"),
                Environment.GetEnvironmentVariable("CODEASSIST_USE_INDEX_WORKER") == "1", shutdown.Token);
        }
        public void Dispose() { shutdown.Cancel(); }
```

 CodeAssist.Extension/ProjectIndexService.cs:101

```
            if (string.IsNullOrWhiteSpace(unrealProjectPath)) return;
            var extensions=FileExtensionSettings.Parse(StartupUi.Preferences.CustomFileExtensions);
            var projectRoot = Path.GetDirectoryName(unrealProjectPath);
            var engineRoot = string.Equals(Path.GetExtension(unrealProjectPath),".uproject",StringComparison.OrdinalIgnoreCase) ? FindEngineRoot(unrealProjectPath) : null;
            var projectCachePath = GetProjectCachePath(projectRoot);
            var engineCachePath = GetEngineCachePath(engineRoot);
```

### 추가 실행 대조

- 설정/재시작·프로젝트 시작/편집·검사동시성·실제CPU

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

VA-F108 · Options Dialog

부분 대응·제한 기록 / 전체 동등성 미검증

### VA 문서의 요구

[공식 문서](https://www.wholetomato.com/documentation/configuration/options-dialog)

- 도구 창 문맥 메뉴에도 옵션 존재

- 다른 IDE 인스턴스는 재시작 전 변경 미반영

- 문서 screenshot은 최신 VA 기준일 수 있음

### CA의 현재 범위·차이

작동하는 옵션과 미지원 범주 표시가 공존합니다. 모든 VA 옵션 및 기본값이 구현된 상태가 아닙니다.

### 현재 코드 근거 후보

 CodeAssist.Extension/ProductOptions.cs:226

```
                content.Children.Add(new Border { Background = new SolidColorBrush(Color.FromArgb(30,255,211,70)), Padding = new Thickness(12),
                    Child = ProductTheme.Text(UiText.Get("이 범주는 현재 지원하지 않습니다. 제공되는 기능은 관련 설정에서 사용할 수 있습니다.", StartupUi.Preferences.UiLanguage)) });
            }
            void Page(TreeViewItem parent, string section, string title, Action body)
            {
                var item = new TreeViewItem { Header = UiText.Get(title, StartupUi.Preferences.UiLanguage), Tag = section + " / " + title, Padding = new Thickness(4,5,0,5) };
                parent.Items.Add(item); pages[(string)item.Tag] = () => { Heading(section,title); body(); };
```

 CodeAssist.Extension/ProductOptions.cs:290

```
            });
            Page(editor,"Editor","Floating Elements",()=>Unavailable(UiText.Get("빠른 작업은 Alt+Shift+Q로 엽니다. 부동 빠른 작업 툴바는 현재 지원하지 않습니다.", StartupUi.Preferences.UiLanguage)));
            Page(editor,"Editor","Smart Select",()=> {
                Toggle(UiText.Get("Start and grow selections by relatively small increments",StartupUi.Preferences.UiLanguage),draft.SmartSelectSmallIncrements,v=>draft.SmartSelectSmallIncrements=v);
                var word=Toggle(UiText.Get("Start non-block selections with the current word",StartupUi.Preferences.UiLanguage),draft.SmartSelectStartWithWord,v=>draft.SmartSelectStartWithWord=v);
                var casing=Toggle(UiText.Get("Changes in case delimit current word",StartupUi.Preferences.UiLanguage),draft.SmartSelectSplitCase,v=>draft.SmartSelectSplitCase=v);
                var underscore=Toggle(UiText.Get("Underscores delimit current word",StartupUi.Preferences.UiLanguage),draft.SmartSelectSplitUnderscores,v=>draft.SmartSelectSplitUnderscores=v);
```

 CodeAssist.Core/Localization/ui.xml:431

```
  </entry>
  <entry key="ui.d0db8b5e364b" source="Options">
    <text lang="ko">설정</text>
    <text lang="en">Options</text>
  </entry>
  <entry key="ui.c205924de0fe" source="Documentation">
    <text lang="ko">문서</text>
```

 CodeAssist.Core/Localization/ui.xml:433

```
    <text lang="ko">설정</text>
    <text lang="en">Options</text>
  </entry>
  <entry key="ui.c205924de0fe" source="Documentation">
    <text lang="ko">문서</text>
    <text lang="en">Documentation</text>
  </entry>
```

 CodeAssist.Core/Localization/ui.xml:439

```
  </entry>
  <entry key="ui.700fe314615c" source="CodeAssist Options">
    <text lang="ko">CodeAssist 설정</text>
    <text lang="en">CodeAssist Options</text>
  </entry>
  <entry key="ui.completion.native.colors" source="Visual Studio 기본 추천 색상 사용">
    <text lang="ko">Visual Studio 기본 추천 색상 사용</text>
```

### 추가 실행 대조

Undo·설정 기본값·언어/VS 버전·성능 전체 검증이 남아 있습니다.

## 원문과 편집 이력

[보존된 원문 열기](../assets/imports/docs/all-108-review-20261017.html)

- 원문 SHA-256: `f4389820516b1502c06c18067a42e1088c8e05d5121ced46416fdfa7ddaab1d8`
- 표시 날짜는 원본 파일의 수정 시각입니다. 작업 완료 시각은 원문에 있는 값을 기준으로 확인합니다.
