package kabam.rotmg.messaging.impl.outgoing {
import flash.utils.IDataOutput;

public class MarketCreate extends OutgoingMessage {

    public function MarketCreate(_arg_1:uint, _arg_2:Function) {
        super(_arg_1, _arg_2);
    }

    public var includedSlots_:Vector.<int>;
    public var requestItems_:Vector.<int>
    public var requestData_:Vector.<Object>;

    override public function writeToOutput(_arg1:IDataOutput):void {
        _arg1.writeInt(includedSlots_.length);
        for each(var i:int in includedSlots_) {
            _arg1.writeInt(i);
        }
        _arg1.writeInt(requestItems_.length);
        for each(var j:int in requestItems_) {
            _arg1.writeInt(j);
        }
        _arg1.writeInt(requestData_.length);
        for each(var k:Object in requestData_) {
            _arg1.writeUTF(JSON.stringify(k));
        }
    }

    override public function toString():String {
        return formatToString("MARKETCREATE", "includedSlots_", "requestItems_", "requestData_");
    }
}
}
