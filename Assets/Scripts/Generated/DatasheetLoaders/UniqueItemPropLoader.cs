
// It's generated file. DO NOT MODIFY IT!

using Diablerie.Engine.Datasheets;
using Diablerie.Engine.IO.D2Formats;

class UniqueItemPropLoader : Datasheet.Loader<UniqueItem.Prop>
{

    public void LoadRecord(ref UniqueItem.Prop record, DatasheetStream stream)
    {
                stream.Read(ref record.prop);
                stream.Read(ref record.param);
                stream.Read(ref record.min);
                stream.Read(ref record.max);
    }
}
