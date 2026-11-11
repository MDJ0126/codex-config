import {call,connect,persistentClient} from './connection.mjs';
const [tool='list_companions',argumentJson='{}',label='codex']=process.argv.slice(2);
try {
  const connection=await connect();
  const args=JSON.parse(argumentJson);
  const result=await call(tool,args,persistentClient(label),connection);
  process.stdout.write(JSON.stringify(result,null,2)+'\n');
} catch(error) {process.stderr.write(error.message+'\n');process.exitCode=1;}
