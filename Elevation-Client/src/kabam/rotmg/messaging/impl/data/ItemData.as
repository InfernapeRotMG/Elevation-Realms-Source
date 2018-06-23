package kabam.rotmg.messaging.impl.data {

import flash.utils.ByteArray;

public class ItemData {

    public var objectType:int;
    public var itemStack:int;

    public function ItemData(obj:Object) {
        objectType = getValueOrDefault(obj, "ObjectType", -1);
        itemStack = getValueOrDefault(obj, "ItemStack", -1);
    }

    public static function defaultData():ItemData {
        return new ItemData({
            "ObjectType": -1,
            "ItemStack": -1
        });
    }

    public function isCustom():Boolean {
        return !(itemStack == -1);
    }

    public function toJson():String {
        return JSON.stringify(this);
    }

    public static function fromJson(json:String):ItemData {
        return new ItemData(JSON.parse(json));
    }

    private static function getValueOrDefault(obj:Object, name:String, defaultValue:Object=null):* {
        return obj.hasOwnProperty(name) ? obj[name] : defaultValue;
    }

    public function copy():ItemData {
        var tmpArray:ByteArray = new ByteArray();
        tmpArray.writeObject(this);
        tmpArray.position = 0;
        return new ItemData(tmpArray.readObject());
    }
}
}