const languages={
 js:'javascript',javascript:'javascript',ts:'typescript',typescript:'typescript',
 cs:'csharp',csharp:'csharp','c#':'csharp',ps1:'powershell',powershell:'powershell',
 sh:'bash',shell:'bash',bash:'bash',json:'json',css:'css',html:'xml',xml:'xml',
 cpp:'cpp','c++':'cpp',c:'c',python:'python',py:'python',yaml:'yaml',yml:'yaml',
 diff:'diff',sql:'sql',ini:'ini',markdown:'markdown',md:'markdown'
};
const cdn='https://cdn.jsdelivr.net/npm/@highlightjs/cdn-assets@11.11.1/es';
const blocks=[...document.querySelectorAll('.post-body pre > code[data-language]')];
const groups=new Map();
for(const block of blocks){
 const tag=block.dataset.language;
 if(!Object.hasOwn(languages,tag))continue;
 const language=languages[tag];
 if(!groups.has(language))groups.set(language,[]);
 groups.get(language).push(block);
}
if(groups.size){
 try{
  const {default:hljs}=await import(cdn+'/core.min.js');
  await Promise.allSettled([...groups].map(async ([language,nodes])=>{
   const {default:grammar}=await import(cdn+'/languages/'+language+'.min.js');
   hljs.registerLanguage(language,grammar);
   for(const node of nodes){
    node.className='language-'+language;
    hljs.highlightElement(node);
   }
  }));
 }catch{
  // Keep original text readable when the CDN is unavailable.
 }
}
const diagrams=[...document.querySelectorAll('.post-body pre.mermaid')];
if(diagrams.length){
 try{
  const {default:mermaid}=await import('https://cdn.jsdelivr.net/npm/mermaid@12.3.0/dist/mermaid.esm.min.mjs');
  mermaid.initialize({startOnLoad:false,securityLevel:'strict',htmlLabels:false,theme:'neutral',suppressErrorRendering:true});
  for(const diagram of diagrams){
   const source=diagram.textContent;
   try{await mermaid.run({nodes:[diagram]});}
   catch{diagram.textContent=source;}
  }
 }catch{
  // The original description remains above the diagram.
 }
}
