import{test}from'node:test';import assert from'node:assert/strict';import{projectFor}from'./resolve-thread.mjs';
const projects=[{projectId:'p',projectKind:'local',hostId:'local',path:'C:\\work\\config',label:'전역 지시사항 관리'},{projectId:'nested',projectKind:'local',hostId:'local',path:'C:\\work\\config\\child',label:'하위 프로젝트'}];
test('project association uses ID, registered path fallback, deepest root and boundaries',()=>{
 assert.equal(projectFor({projectId:'p',cwd:'C:\\other'},projects),'전역 지시사항 관리');
 assert.equal(projectFor({cwd:'\\\\?\\C:\\work\\config'},projects),'전역 지시사항 관리');
 assert.equal(projectFor({cwd:'C:\\work\\config\\child\\src'},projects),'하위 프로젝트');
 assert.equal(projectFor({cwd:'C:\\work\\config-other'},projects),null);
 assert.equal(projectFor({projectId:'missing',cwd:'C:\\work\\config'},projects),'프로젝트명 미확인');
});
