
// It's generated file. DO NOT MODIFY IT!

using Diablerie.Engine.Datasheets;
using Diablerie.Engine.IO.D2Formats;

class ItemSetPropLoader : Datasheet.Loader<ItemSet.Prop>
{

    public void LoadRecord(ref ItemSet.Prop record, DatasheetStream stream)
    {
                stream.Read(ref record.prop);
                stream.Read(ref record.param);
                stream.Read(ref record.min);
                stream.Read(ref record.max);
    }
}
