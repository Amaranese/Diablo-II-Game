
// It's generated file. DO NOT MODIFY IT!

using Diablerie.Engine.Datasheets;
using Diablerie.Engine.IO.D2Formats;

class ObjectInfoLoader : Datasheet.Loader<ObjectInfo>
{

    public void LoadRecord(ref ObjectInfo record, DatasheetStream stream)
    {
                stream.Read(ref record.nameStr);
                stream.Read(ref record.description);
                stream.Read(ref record.id);
                stream.Read(ref record.token);
                stream.Read(ref record.spawnMax);
                record.selectable = new bool[8];
                    stream.Read(ref record.selectable[0]);
                    stream.Read(ref record.selectable[1]);
                    stream.Read(ref record.selectable[2]);
                    stream.Read(ref record.selectable[3]);
                    stream.Read(ref record.selectable[4]);
                    stream.Read(ref record.selectable[5]);
                    stream.Read(ref record.selectable[6]);
                    stream.Read(ref record.selectable[7]);
                stream.Read(ref record.trapProb);
                stream.Read(ref record.sizeX);
                stream.Read(ref record.sizeY);
                stream.Read(ref record.nTgtFX);
                stream.Read(ref record.nTgtFY);
                stream.Read(ref record.nTgtBX);
                stream.Read(ref record.nTgtBY);
                record.frameCount = new int[8];
                    stream.Read(ref record.frameCount[0]);
                    stream.Read(ref record.frameCount[1]);
                    stream.Read(ref record.frameCount[2]);
                    stream.Read(ref record.frameCount[3]);
                    stream.Read(ref record.frameCount[4]);
                    stream.Read(ref record.frameCount[5]);
                    stream.Read(ref record.frameCount[6]);
                    stream.Read(ref record.frameCount[7]);
                record.frameDelta = new int[8];
                    stream.Read(ref record.frameDelta[0]);
                    stream.Read(ref record.frameDelta[1]);
                    stream.Read(ref record.frameDelta[2]);
                    stream.Read(ref record.frameDelta[3]);
                    stream.Read(ref record.frameDelta[4]);
                    stream.Read(ref record.frameDelta[5]);
                    stream.Read(ref record.frameDelta[6]);
                    stream.Read(ref record.frameDelta[7]);
                record.cycleAnim = new bool[8];
                    stream.Read(ref record.cycleAnim[0]);
                    stream.Read(ref record.cycleAnim[1]);
                    stream.Read(ref record.cycleAnim[2]);
                    stream.Read(ref record.cycleAnim[3]);
                    stream.Read(ref record.cycleAnim[4]);
                    stream.Read(ref record.cycleAnim[5]);
                    stream.Read(ref record.cycleAnim[6]);
                    stream.Read(ref record.cycleAnim[7]);
                record.lit = new int[8];
                    stream.Read(ref record.lit[0]);
                    stream.Read(ref record.lit[1]);
                    stream.Read(ref record.lit[2]);
                    stream.Read(ref record.lit[3]);
                    stream.Read(ref record.lit[4]);
                    stream.Read(ref record.lit[5]);
                    stream.Read(ref record.lit[6]);
                    stream.Read(ref record.lit[7]);
                record.blocksLight = new bool[8];
                    stream.Read(ref record.blocksLight[0]);
                    stream.Read(ref record.blocksLight[1]);
                    stream.Read(ref record.blocksLight[2]);
                    stream.Read(ref record.blocksLight[3]);
                    stream.Read(ref record.blocksLight[4]);
                    stream.Read(ref record.blocksLight[5]);
                    stream.Read(ref record.blocksLight[6]);
                    stream.Read(ref record.blocksLight[7]);
                record.hasCollision = new bool[8];
                    stream.Read(ref record.hasCollision[0]);
                    stream.Read(ref record.hasCollision[1]);
                    stream.Read(ref record.hasCollision[2]);
                    stream.Read(ref record.hasCollision[3]);
                    stream.Read(ref record.hasCollision[4]);
                    stream.Read(ref record.hasCollision[5]);
                    stream.Read(ref record.hasCollision[6]);
                    stream.Read(ref record.hasCollision[7]);
                stream.Read(ref record.isAttackable);
                record.start = new int[8];
                    stream.Read(ref record.start[0]);
                    stream.Read(ref record.start[1]);
                    stream.Read(ref record.start[2]);
                    stream.Read(ref record.start[3]);
                    stream.Read(ref record.start[4]);
                    stream.Read(ref record.start[5]);
                    stream.Read(ref record.start[6]);
                    stream.Read(ref record.start[7]);
                stream.Read(ref record.envEffect);
                stream.Read(ref record.isDoor);
                stream.Read(ref record.blockVis);
                stream.Read(ref record.orientation);
                stream.Read(ref record.trans);
                record.orderFlag = new int[8];
                    stream.Read(ref record.orderFlag[0]);
                    stream.Read(ref record.orderFlag[1]);
                    stream.Read(ref record.orderFlag[2]);
                    stream.Read(ref record.orderFlag[3]);
                    stream.Read(ref record.orderFlag[4]);
                    stream.Read(ref record.orderFlag[5]);
                    stream.Read(ref record.orderFlag[6]);
                    stream.Read(ref record.orderFlag[7]);
                stream.Read(ref record.preOperate);
                record.mode = new bool[8];
                    stream.Read(ref record.mode[0]);
                    stream.Read(ref record.mode[1]);
                    stream.Read(ref record.mode[2]);
                    stream.Read(ref record.mode[3]);
                    stream.Read(ref record.mode[4]);
                    stream.Read(ref record.mode[5]);
                    stream.Read(ref record.mode[6]);
                    stream.Read(ref record.mode[7]);
                stream.Read(ref record.yOffset);
                stream.Read(ref record.xOffset);
                stream.Read(ref record.draw);
                stream.Read(ref record.red);
                stream.Read(ref record.blue);
                stream.Read(ref record.green);
                record.layersSelectable = new bool[16];
                    stream.Read(ref record.layersSelectable[0]);
                    stream.Read(ref record.layersSelectable[1]);
                    stream.Read(ref record.layersSelectable[2]);
                    stream.Read(ref record.layersSelectable[3]);
                    stream.Read(ref record.layersSelectable[4]);
                    stream.Read(ref record.layersSelectable[5]);
                    stream.Read(ref record.layersSelectable[6]);
                    stream.Read(ref record.layersSelectable[7]);
                    stream.Read(ref record.layersSelectable[8]);
                    stream.Read(ref record.layersSelectable[9]);
                    stream.Read(ref record.layersSelectable[10]);
                    stream.Read(ref record.layersSelectable[11]);
                    stream.Read(ref record.layersSelectable[12]);
                    stream.Read(ref record.layersSelectable[13]);
                    stream.Read(ref record.layersSelectable[14]);
                    stream.Read(ref record.layersSelectable[15]);
                stream.Read(ref record.totalPieces);
                stream.Read(ref record.subClass);
                stream.Read(ref record.xSpace);
                stream.Read(ref record.ySpace);
                stream.Read(ref record.nameOffset);
                stream.Read(ref record.monsterOk);
                stream.Read(ref record.operateRange);
                stream.Read(ref record.shrineFunction);
                stream.Read(ref record.restore);
                record.parm = new int[8];
                    stream.Read(ref record.parm[0]);
                    stream.Read(ref record.parm[1]);
                    stream.Read(ref record.parm[2]);
                    stream.Read(ref record.parm[3]);
                    stream.Read(ref record.parm[4]);
                    stream.Read(ref record.parm[5]);
                    stream.Read(ref record.parm[6]);
                    stream.Read(ref record.parm[7]);
                stream.Read(ref record.act);
                stream.Read(ref record.lockable);
                stream.Read(ref record.gore);
                stream.Read(ref record.sync);
                stream.Read(ref record.flicker);
                stream.Read(ref record.damage);
                stream.Read(ref record.beta);
                stream.Read(ref record.overlay);
                stream.Read(ref record.collisionSubst);
                stream.Read(ref record.left);
                stream.Read(ref record.top);
                stream.Read(ref record.width);
                stream.Read(ref record.height);
                stream.Read(ref record.operateFn);
                stream.Read(ref record.populateFn);
                stream.Read(ref record.initFn);
                stream.Read(ref record.clientFn);
                stream.Read(ref record.restoreVirgins);
                stream.Read(ref record.blocksMissile);
                stream.Read(ref record.drawUnder);
                stream.Read(ref record.openWarp);
                stream.Read(ref record.autoMap);
    }
}
