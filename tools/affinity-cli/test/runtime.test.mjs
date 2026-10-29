// Control-flow verification only. These doubles do not verify Affinity's native
// engine or image quality; real application integration is recorded separately.
import test from 'node:test';
import assert from 'node:assert/strict';
import vm from 'node:vm';
import { affinityRuntime } from '../src/jobs.mjs';

function runNative(job, options = {}) {
  const edits = [], files = new Set(options.files ?? []);
  const box = {x:0,y:0,width:10,height:10};
  const nodes = options.nodes ?? [];
  const document = { sessionUuid:'target',title:'Test',path:'test.af',widthPixels:800,heightPixels:600,dpi:96,isOpen:true,isReadOnly:false,
    layers:{all:nodes},currentSpread:{},format:'RGBA8',isDirty:false,
    executeCommand(command){ edits.push('insert'); for(const node of command.newNodes) nodes.push(node); return 0; },
    saveAs(filename){edits.push('save');files.add(filename);return 0;},
    setBrushFillDescriptor(){edits.push('fill');return 0;},
    export(){return {all:options.exportRecords ?? []};}
  };
  const modules = {
    '/application.js':{Application:{version:'test'}},
    '/document.js':{Document:{all:options.noDocument?[]:[document],current:null},FileExportOptions:{allPresetNames:['PNG'],createWithPresetName:()=>({})},FileExportArea:{createForWholeDocument:()=>({})}},
    '/nodes.js':{ShapeNodeDefinition:{create:()=>({})}},
    '/shapes.js':{ShapeRectangle:{create:()=>({})},ShapeEllipse:{create:()=>({})}},
    '/colours.js':{Colour:{createRGBA8:x=>x}},
    '/geometry.js':{Rectangle:class {constructor(x,y,width,height){Object.assign(this,{x,y,width,height});}}},
    '/storybuilder.js':{},'/fonts.js':{Font:{all:[]}},'/fills.js':{},'/rasterobject.js':{},
    '/commands.js':{AddChildNodesCommandBuilder:{create:()=>({setInsertionTarget(){},addNode(definition){this.definition=definition;},
      createCommand(){return {newNodes:[{userDescription:this.definition.userDescription,description:'Shape',spreadBaseBox:box}]};}})}},
    '/fs.js':{fs:{exists:filename=>files.has(filename)}}
  };
  const context={job,require:name=>{if(!modules[name])throw new Error('Unknown module '+name);return modules[name];}};
  const result=vm.runInNewContext(`(${affinityRuntime.toString()})(job)`,context);
  return {result,edits};
}
const target=steps=>({version:1,document:{id:'target'},steps});
test('native job refuses a stale session id without editing the active document',()=>{
  const {result,edits}=runNative(target([{op:'inspect'}]),{noDocument:true});
  assert.equal(result.ok,false);assert.match(result.error,/not open/);assert.equal(edits.length,0);
});
test('native job preflights fonts and does not silently substitute them',()=>{
  const {result,edits}=runNative(target([
    {op:'shape',x:0,y:0,width:10,height:10,fill:'#FFFFFF'},
    {op:'text',x:0,y:0,text:'x',size:12,font:'Missing',fill:'#000000'}
  ]));
  assert.equal(result.ok,false);assert.match(result.error,/font not installed/);assert.equal(edits.length,0);
});
test('native job rechecks existing outputs and unique layer names before edits',()=>{
  const {result,edits}=runNative(target([{op:'save',path:'existing.af'}]),{files:['existing.af']});
  assert.equal(result.ok,false);assert.equal(edits.length,0);
  const duplicate=runNative(target([{op:'rename',layer:'Same',name:'Changed'}]),{nodes:[{userDescription:'Same'},{userDescription:'Same'}]});
  assert.equal(duplicate.result.ok,false);assert.equal(duplicate.edits.length,0);
});
test('native job reports previous changes when a later operation fails',()=>{
  const {result,edits}=runNative(target([
    {op:'shape',name:'Shape',x:0,y:0,width:10,height:10,fill:'#FFFFFF'},
    {op:'export',path:'export.png',preset:'PNG'}
  ]),{exportRecords:[{isSuccess:false,errorMessage:'Denied'}]});
  assert.equal(result.ok,false);assert.match(result.error,/Export failed/);
  assert.equal(result.partialChangesPossible,true);assert.equal(result.completed[0].op,'shape');assert.deepEqual(edits,['insert']);
});
