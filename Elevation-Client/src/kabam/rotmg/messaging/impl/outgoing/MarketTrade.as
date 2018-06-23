                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           package kabam.rotmg.messaging.impl.outgoing {
import flash.utils.IDataOutput;

public class MarketTrade extends OutgoingMessage {

    public function MarketTrade(_arg_1:uint, _arg_2:Function) {
        super(_arg_1, _arg_2);
    }

    public var tradeId_:int;

    override public function writeToOutput(_arg1:IDataOutput):void {
        _arg1.writeInt(this.tradeId_);
    }

    override public function toString():String {
        return formatToString("MARKETTRADE", "tradeId_");
    }
}
}
