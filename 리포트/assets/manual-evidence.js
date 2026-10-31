// Expand actual/original evidence without changing any stored QA verdict.
document.addEventListener('click',event=>{const expand=event.target.closest('[data-manual-expand]'),collapse=event.target.closest('[data-manual-collapse]');if(expand||collapse)document.querySelectorAll('.manual-task').forEach(task=>{task.open=Boolean(expand);});});
const reveal=()=>{const target=document.getElementById(decodeURIComponent(location.hash.slice(1)));if(target?.classList.contains('manual-task'))target.open=true;};addEventListener('hashchange',reveal);reveal();
