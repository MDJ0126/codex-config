import fs from 'node:fs/promises';
import path from 'node:path';
import crypto from 'node:crypto';
import {spawn} from 'node:child_process';

export function inspectWave(wave) {
  if(wave.length<44 || wave.length>960044 || wave.toString('ascii',0,4)!=='RIFF' || wave.toString('ascii',8,12)!=='WAVE' || wave.toString('ascii',12,16)!=='fmt ' || wave.readUInt32LE(16)!==16 || wave.readUInt16LE(20)!==1 || wave.readUInt16LE(22)!==1 || wave.readUInt32LE(24)!==16000 || wave.readUInt16LE(34)!==16 || wave.toString('ascii',36,40)!=='data' || wave.readUInt32LE(40)!==wave.length-44 || (wave.length-44)%2!==0) throw new Error('16kHz mono PCM16 WAV, maximum 30 seconds, required.');
  const count=(wave.length-44)/2;let energy=0;
  for(let offset=44;offset<wave.length;offset+=2) energy+=(wave.readInt16LE(offset)/32768)**2;
  return {seconds:count/16000,rms:count?Math.sqrt(energy/count):0};
}

export class LocalSpeech {
  constructor(directory) {this.directory=directory;this.busy=false;}
  async configuration() {
    const config=JSON.parse(await fs.readFile(path.join(this.directory,'speech.json'),'utf8'));
    if(!path.isAbsolute(config.executable)||!path.isAbsolute(config.model)||!['ko','en','auto'].includes(config.language)||!config.executable.endsWith('.exe')) throw new Error('Invalid local speech configuration.');
    await Promise.all([fs.access(config.executable),fs.access(config.model)]);return config;
  }
  async status() {
    try {const config=await this.configuration();return {ready:true,language:config.language,model:'Whisper base multilingual',busy:this.busy};}
    catch {return {ready:false,note:'Run pwsh -File Tools/Install-LocalSpeech.ps1'};}
  }
  async transcribe(wave) {
    if(this.busy) throw new Error('음성 인식이 진행 중입니다. 잠시 후 다시 시도해 주세요.');
    const audio=inspectWave(wave);
    if(audio.seconds<0.25 || audio.rms<0.003) throw new Error('목소리가 너무 짧거나 작습니다. 다시 녹음해 주세요.');
    this.busy=true;const started=performance.now();
    const temporary=path.join(this.directory,'audio-tmp',crypto.randomUUID());
    try {
      const config=await this.configuration();
      await fs.mkdir(path.dirname(temporary),{recursive:true});await fs.writeFile(temporary+'.wav',wave);
      await new Promise((resolve,reject)=>{
        const child=spawn(config.executable,['-m',config.model,'-f',temporary+'.wav','-l',config.language,'-t','4','-otxt','-of',temporary,'-np','-nt'],{windowsHide:true,stdio:['ignore','ignore','pipe'],shell:false});
        let error='';const timer=setTimeout(()=>{child.kill();reject(new Error('로컬 음성 인식 시간이 초과됐습니다.'));},90000);
        child.stderr.on('data',chunk=>{error=(error+chunk).slice(-2000);});
        child.once('error',failure=>{clearTimeout(timer);reject(failure);});
        child.once('exit',code=>{clearTimeout(timer);code===0?resolve():reject(new Error('음성 인식 실행에 실패했습니다. 설치 상태를 확인해 주세요.'));});
      });
      const text=(await fs.readFile(temporary+'.txt','utf8')).trim().slice(0,1000);
      if(!text || /^\[.*\]$/.test(text)) throw new Error('인식할 말을 찾지 못했습니다. 다시 녹음해 주세요.');
      return {text,requestMs:performance.now()-started,audioSeconds:audio.seconds,requiresPlayerConfirmation:true};
    } catch(error) {
      if(error.code==='ENOENT') throw new Error('로컬 음성 인식을 먼저 설치해 주세요: Tools/Install-LocalSpeech.ps1');
      throw error;
    } finally {
      await Promise.all(['.wav','.txt'].map(extension=>fs.rm(temporary+extension,{force:true})));this.busy=false;
    }
  }
}
