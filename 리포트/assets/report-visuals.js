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
  const root=document.documentElement;
  const sources=diagrams.map(diagram=>diagram.textContent);
  let renderedTheme;
  let rendering=Promise.resolve();

  async function renderDiagrams(){
   const theme=root.dataset.theme;
   if(theme===renderedTheme)return;
   renderedTheme=theme;
   const style=getComputedStyle(diagrams[0]);
   const color=name=>style.getPropertyValue(name).trim();
   mermaid.initialize({
    startOnLoad:false,
    securityLevel:'strict',
    htmlLabels:false,
    suppressErrorRendering:true,
    theme:'base',
    look:'classic',
    fontFamily:'Segoe UI, Malgun Gothic, system-ui, sans-serif',
    themeCSS:'.node rect { rx: 6px; }',
    themeVariables:{
     darkMode:theme==='dark',
     fontSize:'14px',
     background:color('--paper'),
     primaryColor:color('--diagram-node'),
     primaryBorderColor:color('--diagram-border'),
     primaryTextColor:color('--ink'),
     lineColor:color('--green'),
     edgeLabelBackground:color('--paper')
    },
    flowchart:{curve:'linear',nodeSpacing:28,rankSpacing:30,padding:10,wrappingWidth:115}
   });
   for(const [index,diagram] of diagrams.entries()){
    const scrollLeft=diagram.scrollLeft;
    const source=sources[index];
    diagram.textContent=source;
    diagram.removeAttribute('data-processed');
    try{
     await mermaid.run({nodes:[diagram]});
     const svg=diagram.querySelector('svg');
     if(svg)svg.style.minWidth=`${svg.viewBox.baseVal.width}px`;
     diagram.scrollLeft=scrollLeft;
    }catch{
     diagram.textContent=source;
    }
   }
  }
  const themeObserver=new MutationObserver(()=>{
   rendering=rendering.then(renderDiagrams);
  });
  themeObserver.observe(root,{attributes:true,attributeFilter:['data-theme']});
  rendering=renderDiagrams();
  await rendering;
 }catch{
  // The original description remains above the diagram.
 }
}
