import {escapeHTML as esc} from './render.mjs';
export const copyButton=(url='',label='링크 복사',title='')=>`<button type="button" class="copy-link" data-copy-url="${esc(url)}" aria-label="${esc(title?title+' 링크 복사':label)}">${esc(label)}</button>`;
