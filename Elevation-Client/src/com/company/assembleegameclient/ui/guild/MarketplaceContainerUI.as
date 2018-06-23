// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_sP_._07x

package com.company.assembleegameclient.ui.guild {


import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.game.events.GuildResultEvent;

import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.screens.TitleMenuOption;
import com.company.assembleegameclient.ui.dialogs.Dialog;

import flash.display.Sprite;

    import com.company.assembleegameclient.game.GameSprite;

    import com.company.rotmg.graphics.ScreenGraphic;

    import flash.events.MouseEvent;
    import flash.events.Event;

    import com.company.assembleegameclient.objects.Player;


    import flash.events.KeyboardEvent;
    import flash.text.TextField;

    public class MarketplaceContainerUI extends Sprite {

        private var gs_:AGameSprite;
        private var marketPlace_:Marketplace;
        private var create_:MarketplaceCreate;
        private var close_:TitleMenuOption;
        private var newTrade_:TitleMenuOption;
        private var category_:String = "all";

        public function MarketplaceContainerUI(_arg_1:AGameSprite) {
            this.gs_ = _arg_1;
            graphics.clear();
            graphics.beginFill(0x2B2B2B, 0.8);
            graphics.drawRect(0, 0, 800, 600);
            graphics.endFill();
            this.addMarket();
            addChild(new ScreenGraphic());
            this.close_ = new TitleMenuOption("close", 36, false);
            this.close_.addEventListener(MouseEvent.CLICK, this.closeThis);
            this.newTrade_ = new TitleMenuOption("create", 26, false);
            this.newTrade_.addEventListener(MouseEvent.CLICK, this.create);
            addChild(this.close_);
            addChild(this.newTrade_);
            addEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
            addEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
        }

        private function addMarket():void {
            this.marketPlace_ = new Marketplace(50, 0, this.gs_,  this.category_);
            this.marketPlace_.addEventListener(MarketplaceEvent.TRADE, this.onTrade);
            addChild(this.marketPlace_);
        }

        private function _L_I_(_arg1:String):void {
            var _local2:Dialog = new Dialog(_arg1, "Error", "Ok", null, "/marketError");
            _local2.addEventListener(Dialog.LEFT_BUTTON, this._K__);
            stage.addChild(_local2);
        }

        private function _C_L_(_arg1:GuildResultEvent):void {
            this.gs_.removeEventListener(GuildResultEvent.market_, this._C_L_);
            if (!_arg1.success_) {
                this._L_I_(_arg1.errorKey);
            } else {
                this.addMarket();
            }
        }

        private function _X_s(_arg1:GuildResultEvent):void {
            this.gs_.removeEventListener(GuildResultEvent.market_, this._X_s);
            if (!_arg1.success_) {
                this._L_I_(_arg1.errorKey);
            } else {
                this.addMarket();
            }
        }

        private function _K__(_arg1:Event):void {
            var _local2:Dialog = (_arg1.currentTarget as Dialog);
            stage.removeChild(_local2);
            this.addMarket();
        }

        private function closeThis(_arg1:MouseEvent):void {
            this.close();
        }

        private function close():void {
            stage.focus = null;
            parent.removeChild(this);
        }

        private function onAddedToStage(_arg1:Event):void {
            stage;
            this.close_.x = ((800 / 2) - (this.close_.width / 2) - 60);
            this.close_.y = 526;
            this.newTrade_.x = this.close_.x + this.close_.width + 100;
            this.newTrade_.y = 535;
            stage.addEventListener(KeyboardEvent.KEY_DOWN, this._0A_Y_, false, 1);
            stage.addEventListener(KeyboardEvent.KEY_UP, this._H_H_, false, 1);
        }

        private function onRemovedFromStage(_arg1:Event):void {
            stage.removeEventListener(KeyboardEvent.KEY_DOWN, this._0A_Y_, false);
            stage.removeEventListener(KeyboardEvent.KEY_UP, this._H_H_, false);
        }

        private function _0A_Y_(_arg1:KeyboardEvent):void {
            _arg1.stopImmediatePropagation();
        }

        private function _H_H_(_arg1:KeyboardEvent):void {
            _arg1.stopImmediatePropagation();
        }

        private function onTrade(event:MarketplaceEvent):void {
            this.category_ = this.marketPlace_.category_;
            this.marketPlace_.removeEventListener(MarketplaceEvent.TRADE, this.onTrade);
            removeChild(this.marketPlace_);
            this.gs_.gsc_.marketTrade(event.id_);
            this.gs_.addEventListener(GuildResultEvent.market_, this._C_L_);
        }

        private function create(event:MouseEvent):void {
            if(this.marketPlace_.visible == false) {
                if(this.create_ != null) {
                    removeChild(this.create_);
                    this.create_ = null;
                }
                this.marketPlace_.visible = true;
                this.marketPlace_.rebuild();

            } else {
                this.marketPlace_.visible = false;
                this.create_ = new MarketplaceCreate(this.gs_);
                addChild(this.create_);
                this.create_.addEventListener(MarketplaceEvent.CREATE, this.createTrade);
            }
        }

        private function search(event:MouseEvent):void {

        }

        private function createTrade(event:MarketplaceEvent):void {
            this.category_ = "mine";
            this.marketPlace_.removeEventListener(MarketplaceEvent.TRADE, this.onTrade);
            removeChild(this.marketPlace_);
            this.create_.removeEventListener(MarketplaceEvent.CREATE, this.createTrade);
            removeChild(this.create_);
            this.gs_.gsc_.marketCreate(event.included_, event.requestItems_, event.requestData_);
            this.gs_.addEventListener(GuildResultEvent.market_, this._C_L_);
        }
    }
}//package _sP_

