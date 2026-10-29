import test from 'node:test';
import assert from 'node:assert/strict';
import {chooseStage} from '../assets/stage-state.mjs';
test('selecting a stage twice restores all reports and preserves project/search',()=>{const initial={stage:'all',group:'all',type:'전체',project:'pixora',query:'palette'},selected=chooseStage(initial,'new');assert.equal(selected.group,'bug');assert.deepEqual(chooseStage(selected,'new'),initial);});
test('all stages clears residual bug group and detail filters',()=>{assert.deepEqual(chooseStage({stage:'resolved',group:'bug',type:'버그',project:'전체'},'all'),{stage:'all',group:'all',type:'전체',project:'전체'});});
test('switching stages retains bug classification',()=>{assert.deepEqual(chooseStage({stage:'new',group:'bug',type:'버그'},'review'),{stage:'review',group:'bug',type:'버그'});});
