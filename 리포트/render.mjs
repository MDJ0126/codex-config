import {Marked} from './vendor/marked/marked.esm.js';
export const escapeHTML=value=>String(value).replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
export function safeURL(value){
 const url=String(value||'').trim();
 if(!url||/[\u0000-\u0020\\]/.test(url)||url.startsWith('//'))return null;
 if(/^[a-z][a-z0-9+.-]*:/i.test(url)&&!/^https?:\/\//i.test(url))return null;
 return url;
}
export function renderMarkdown(source,{bodyMode=false}={}){
 const toc=[];let number=0;
 const parser=new Marked({gfm:true,breaks:false,renderer:{
  html({text}){return escapeHTML(text);},
  heading({tokens,depth}){if(bodyMode)depth=Math.max(2,depth);const html=this.parser.parseInline(tokens),id=`section-${++number}`;if(depth>1)toc.push({id,depth,label:html.replace(/<[^>]*>/g,'')});return `<h${depth} id="${id}">${html}</h${depth}>\n`;},
  link({href,title,tokens}){const text=this.parser.parseInline(tokens),url=safeURL(href);return url?`<a class="report-link" href="${escapeHTML(url)}"${title?` title="${escapeHTML(title)}"`:''}>${text}</a>`:text;},
  image({href,text,title}){const url=safeURL(href);return url?`<figure><img class="report-image" src="${escapeHTML(url)}" alt="${escapeHTML(text)}" loading="lazy">${text?`<figcaption>${escapeHTML(text)}</figcaption>`:''}</figure>`:`<span>${escapeHTML(text)}</span>`;},
  table(token){return `<div class="table-scroll">${Object.getPrototypeOf(this).table.call(this,token)}</div>`;}
 }});
 return {html:parser.parse(source),toc};
}
