/**
 * Created by club5_000 on 9/13/2014.
 */
package com.company.assembleegameclient.account.ui {
import com.company.assembleegameclient.constants.InventoryOwnerTypes;
import com.company.assembleegameclient.game.AGameSprite;

import com.company.assembleegameclient.game.GameSprite;
    import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.screens.TitleMenuOption;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.ui.tooltip.EquipmentToolTip;
    import com.company.util.BitmapUtil;

    import flash.display.Bitmap;
    import flash.display.BitmapData;
    import flash.display.Sprite;
    import flash.events.Event;
    import flash.events.MouseEvent;
    import flash.text.TextFormatAlign;

    public class DataChooser extends Frame {

        public var canceled_:Boolean;
        public var itemType_:int;
        public var itemData_:Object;
        private var gs_:AGameSprite;
        public var itemSprite_:Sprite;
        private var itemBitmap_:Bitmap;
        private var toolTip_:EquipmentToolTip;
        private var Button1:TitleMenuOption;
        private var Button2:TitleMenuOption;

        public function DataChooser(_gs:AGameSprite, _item:int) {
            super("Add item to trade", "Cancel", "OK", null);
            this.gs_ = _gs;
            this.itemType_ = _item;
            this.canceled_ = false;
            this.addIcon();
        }


        private function addIcon() {
            var _local1:XML = ObjectLibrary.xmlLibrary_[this.itemType_];
            var _local2:Number = 5;
            if (_local1.hasOwnProperty("ScaleValue")) {
                _local2 = _local1.ScaleValue;
            }
            //var _local4:int = ObjectLibrary.getItemNameColor(this.itemType_);
            var _local3:BitmapData = ObjectLibrary.getRedrawnTextureFromType(this.itemType_, 60, true, false, _local2);
            if(this.itemData_ != null && this.itemData_.hasOwnProperty("TextureFile") && this.itemData_.TextureFile != "") {
                _local3 = ObjectLibrary.getRedrawnTextureFromType(this.itemType_, 60, true, this.itemData_);
            }
            _local3 = BitmapUtil.cropToBitmapData(_local3, 4, 4, (_local3.width - 8), (_local3.height - 8));
            this.itemSprite_ = new Sprite();
            this.itemBitmap_ = new Bitmap(_local3);
            this.itemBitmap_.scaleX = this.itemBitmap_.scaleY = 2;
            this.itemSprite_.addChild(itemBitmap_);
            this.itemSprite_.addEventListener(MouseEvent.MOUSE_OVER, this.onMouseOver);
            this.itemSprite_.addEventListener(MouseEvent.MOUSE_OUT, this.onMouseOut);
            this.itemSprite_.addEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
            this.addComponent(this.itemSprite_, this.w_ / 2 - this.itemSprite_.width / 2 - 10);


            Button1.addEventListener(MouseEvent.CLICK, this.onClose);
            Button2.addEventListener(MouseEvent.CLICK, this.onComplete)
        }

        private function removeTooltip() {
            if (toolTip_ != null) {
                if (toolTip_.parent != null) {
                    toolTip_.parent.removeChild(toolTip_);
                }
                toolTip_ = null;
            }
        }

        private function onMouseOver(e:MouseEvent):void {
            this.removeTooltip();
           if(this.itemType_ != -1 && ObjectLibrary.getIdFromType(this.itemType_) != null) {
                this.toolTip_ = new EquipmentToolTip(this.itemType_, gs_.map.player_, -1, InventoryOwnerTypes.NPC);
                stage.addChild(this.toolTip_);
            }
        }

        private function onMouseOut(e:MouseEvent) {
            this.removeTooltip();
        }

        private function onRemovedFromStage(e:Event) {
            this.removeTooltip();
        }

        private function onClose(e:Event) {
            stage.focus = null;
            this.canceled_ = true;
            dispatchEvent(new Event(Event.COMPLETE));
        }

        private function onComplete(event:MouseEvent):void {
            stage.focus = null;
            this.itemData_ = {};
            dispatchEvent(new Event(Event.COMPLETE));
        }
    }
}
