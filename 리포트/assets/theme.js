(()=>{
 const button=document.querySelector('#theme-toggle');if(!button)return;
 const root=document.documentElement,media=window.matchMedia('(prefers-color-scheme: dark)');
 const names={system:'시스템',light:'라이트',dark:'다크'},symbols={system:'◐',light:'☀',dark:'☾'};
 function update(preference){root.dataset.themePreference=preference;root.dataset.theme=preference==='dark'||(preference==='system'&&media.matches)?'dark':'light';document.querySelector('#theme-label').textContent=names[preference];button.querySelector('.theme-symbol').textContent=symbols[preference];button.setAttribute('aria-label',`화면 테마: ${names[preference]}. 선택하려면 누르세요`);button.title='시스템 → 라이트 → 다크';}
 button.addEventListener('click',()=>{const modes=['system','light','dark'],next=modes[(modes.indexOf(root.dataset.themePreference)+1)%3];update(next);try{localStorage.setItem('tracefolio-theme',next);}catch{}});
 media.addEventListener('change',()=>{if(root.dataset.themePreference==='system')update('system');});
 update(root.dataset.themePreference||'system');
})();
