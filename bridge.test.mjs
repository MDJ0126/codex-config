import {test} from 'node:test';
import assert from 'node:assert/strict';
import fs from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import crypto from 'node:crypto';
import {openStore,register,resolve,validSignature,localThread} from './bridge.mjs';
import {DatabaseSync} from 'node:sqlite';
test('desktop session remains routable after its historical folder is removed',()=>{
 const dir=fs.mkdtempSync(path.join(os.tmpdir(),'line-session-'));const file=path.join(dir,'state.sqlite');
 const db=new DatabaseSync(file);db.exec('CREATE TABLE threads(id TEXT,cwd TEXT,archived INTEGER)');
 db.prepare('INSERT INTO threads VALUES(?,?,?)').run('live',path.join(dir,'missing-folder'),0);
 db.prepare('INSERT INTO threads VALUES(?,?,?)').run('archived',dir,1);db.close();
 assert.equal(localThread('live',file).id,'live');
 assert.equal(localThread('archived',file),null);assert.equal(localThread('absent',file),null);
 fs.rmSync(dir,{recursive:true,force:true});
});
test('reply routing, latest routing, foreign/missing sessions and persistence',()=>{
 const dir=fs.mkdtempSync(path.join(os.tmpdir(),'line-routing-'));let db=openStore(dir);
 const a='11111111-1111-1111-1111-111111111111',b='22222222-2222-2222-2222-222222222222';
 register(db,{threadId:a,messageIds:['10'],title:'A'});register(db,{threadId:b,messageIds:['20'],title:'B'});
 const event={type:'message',message:{type:'text',text:'request'}};
 assert.equal(resolve(db,event,()=>true).thread_id,b);
 event.message.quotedMessageId='10';assert.equal(resolve(db,event,()=>true).thread_id,a);
 event.message.quotedMessageId='foreign';assert.equal(resolve(db,event,()=>true),null);
 event.message.quotedMessageId='10';assert.equal(resolve(db,event,()=>false),null);
 db.close();db=openStore(dir);assert.equal(resolve(db,event,()=>true).thread_id,a);db.close();
 fs.rmSync(dir,{recursive:true,force:true});
});
test('signature authenticates raw body and rejects tampering',()=>{
 const b=Buffer.from('{"events":[]}');const s=crypto.createHmac('sha256','test-secret').update(b).digest('base64');
 assert.equal(validSignature(b,s,'test-secret'),true);assert.equal(validSignature(Buffer.from('{}'),s,'test-secret'),false);assert.equal(validSignature(b,'','test-secret'),false);
});
