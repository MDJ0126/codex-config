// Keep authored language tags. Infer only when the original text has clear syntax.
export function codeLanguage(text, info=''){
 const language=info.trim().split(/\s+/)[0].toLowerCase();
 if(language)return language;
 if(/^\s*[[{]/.test(text)){
  try{JSON.parse(text);return 'json';}catch{}
 }
 if(/^\s*(?:using\s+[\w.]+;|namespace\s+[\w.]+|\/\/\/ <summary>)/m.test(text)
   && /\b(?:public|private|protected|internal)\s+(?:static\s+)?[\w<>,?]+\s+\w+/m.test(text))return 'csharp';
 if(text.trim().split('\n').every(line=>/^(?:git|node|npm|npx|curl)\s+/.test(line.trim())))return 'bash';
 return '';
}
