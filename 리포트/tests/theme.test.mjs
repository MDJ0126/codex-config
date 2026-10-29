import test from 'node:test';
import assert from 'node:assert/strict';
import vm from 'node:vm';
import {readFile} from 'node:fs/promises';
const init=await readFile(new URL('../assets/theme-init.js',import.meta.url),'utf8');
const script=await readFile(new URL('../assets/theme.js',import.meta.url),'utf8');
function browser(saved,dark=false,deny=false){
 const dataset={},label={},symbol={},events={},attributes={},media={matches:dark,addEventListener:(name,fn)=>events.media=fn};
 const button={querySelector:()=>symbol,addEventListener:(name,fn)=>events.click=fn,setAttribute:(key,value)=>attributes[key]=value};
 const storage={value:saved,getItem(){if(deny)throw Error('denied');return this.value;},setItem(key,value){if(deny)throw Error('denied');this.value=value;}};
 const context=vm.createContext({window:{matchMedia:()=>media},localStorage:storage,document:{documentElement:{dataset},querySelector:selector=>selector==='#theme-toggle'?button:label}});
 vm.runInContext(init,context);vm.runInContext(script,context);
 return {dataset,label,events,media,storage,attributes};
}
test('saved theme wins over OS and the three choices persist',()=>{const b=browser('dark');assert.equal(b.dataset.theme,'dark');b.events.click();assert.equal(b.dataset.themePreference,'system');b.events.click();assert.equal(b.dataset.theme,'light');b.events.click();assert.equal(b.storage.value,'dark');assert.equal(browser(b.storage.value).dataset.theme,'dark');});
test('system mode follows OS changes while explicit light stays light',()=>{const b=browser('system');b.media.matches=true;b.events.media();assert.equal(b.dataset.theme,'dark');b.events.click();assert.equal(b.dataset.theme,'light');b.events.media();assert.equal(b.dataset.theme,'light');});
test('unavailable storage and invalid preferences still allow theme selection',()=>{const b=browser('untrusted',true,true);assert.equal(b.dataset.theme,'dark');assert.equal(b.dataset.themePreference,'system');b.events.click();assert.equal(b.dataset.theme,'light');assert.match(b.attributes['aria-label'],/라이트/);assert.equal(browser('invalid').dataset.themePreference,'system');});
