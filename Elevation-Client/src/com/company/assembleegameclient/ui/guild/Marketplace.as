// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_sP_._if

package com.company.assembleegameclient.ui.guild {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.ClickableTextBase;
import com.company.assembleegameclient.ui.Scrollbar;
import com.company.assembleegameclient.ui.TextButtonBase;
import com.company.ui.BaseSimpleText;
import com.company.util.MoreObjectUtil;

import flash.display.Sprite;
import flash.display.Shape;

    import flash.events.MouseEvent;

    import flash.filters.DropShadowFilter;

import com.company.assembleegameclient.parameters.Parameters;

import flash.display.Graphics;

import flash.events.Event;
import flash.text.TextFieldAutoSize;

import kabam.rotmg.account.core.Account;
import kabam.rotmg.appengine.api.AppEngineClient;
import kabam.rotmg.appengine.impl.AppEngineRetryLoader;
import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;

public class Marketplace extends Sprite {



    public var allButton_:ClickableTextBase;
    public var mineButton_:ClickableTextBase;
    public var searchButton_:ClickableTextBase;
    private var gs_:AGameSprite;
    private var listClient_:AppEngineClient;
    private var loadingText_:TextFieldDisplayConcrete;
    private var titleText_:BaseSimpleText;
    private var _dL_:Shape;
    private var _017:Sprite;
    private var _0A_z:Sprite;
    private var _E_k:Scrollbar;
    private var num_:int;
    private var offset_:int;
    public var category_:String;
    public var search_:MarketplaceSearch;

    public function Marketplace(_arg1:int, _arg2:int, _arg3:AGameSprite, _arg4:String) {
        this.num_ = _arg1;
        this.offset_ = _arg2;
        this.gs_ = _arg3;
        this.category_ = _arg4;
        this.loadingText_ = new TextFieldDisplayConcrete().setSize(22).setColor(0xB3B3B3);
        this.loadingText_.setBold(true);
        this.loadingText_.setStringBuilder(new LineBuilder().setParams(TextKey.LOADING_TEXT));
        this.loadingText_.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
        this.loadingText_.setAutoSize(TextFieldAutoSize.CENTER).setVerticalAlign(TextFieldDisplayConcrete.MIDDLE);
        this.loadingText_.x = (800 / 2);
        this.loadingText_.y = 550;
        addChild(this.loadingText_);
        var _local_4:Account = StaticInjectorContext.getInjector().getInstance(Account);
        var _local_5:Object = {
            "num": _arg1,
            "offset": _arg2,
            "filter": _arg4
        };
        MoreObjectUtil.addToObject(_local_5, _local_4.getCredentials());
        this.listClient_ = StaticInjectorContext.getInjector().getInstance(AppEngineClient);
        this.listClient_.setMaxRetries(2);
        this.listClient_.complete.addOnce(this.onComplete);
        this.listClient_.sendRequest("/market/list", _local_5);

    }

    private function build(_arg_1:XML):void {
        var _local2:Graphics;
        var _local_5:XML;
        var _local9:MarketplaceOffer;
        removeChild(this.loadingText_);
        this.titleText_ = new BaseSimpleText(32, 0xB3B3B3, false, 0, 0);
        this.titleText_.setBold(true);
        this.titleText_.text = "Marketplace";
        this.titleText_.useTextDimensions();
        this.titleText_.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
        this.titleText_.y = 24;
        this.titleText_.x = ((stage.stageWidth / 2) - (this.titleText_.width / 2));
        addChild(this.titleText_);
        this.buildCategoryButtons();
        this._dL_ = new Shape();
        _local2 = this._dL_.graphics;
        _local2.clear();
        _local2.lineStyle(2, 0xEE9327);
        _local2.moveTo(0, 100);
        _local2.lineTo(800, 100);
        _local2.lineStyle();
        addChild(this._dL_);
        this._017 = new Sprite();
        this._017.x = 10;
        this._017.y = 101;
        var _local3:Shape = new Shape();
        _local2 = _local3.graphics;
        _local2.beginFill(0);
        _local2.drawRect(0, 0, MemberListLine.WIDTH, 423);
        _local2.endFill();
        this._017.addChild(_local3);
        this._017.mask = _local3;
        addChild(this._017);
        this._0A_z = new Sprite();
        var _local4:int = 8;
        for each (_local_5 in _arg_1.Offer) {
            _local9 = new MarketplaceOffer(_local_5, this.gs_);
            _local9.x = 4;
            _local9.y = _local4;
            this._0A_z.addChild(_local9);
            _local4 += MarketplaceOffer.HEIGHT + 4;
        }
        this._017.addChild(this._0A_z);
        if (this._0A_z.height > 400) {
            this._E_k = new Scrollbar(16, 400);
            this._E_k.x = ((800 - this._E_k.width) - 4);
            this._E_k.y = 110;
            this._E_k.setIndicatorSize(400, this._0A_z.height);
            this._E_k.addEventListener(Event.CHANGE, this._A_E_);
            addChild(this._E_k);
        }
    }


    public function rebuild():void {
        if(this.category_ == "search") {
            if(this._E_k != null && contains(this._E_k)) {
                removeChild(this._E_k);
                this._E_k = null;
            }
            this._0A_z.y = 0;
            this._0A_z.removeChildren();
            this.search_ = new MarketplaceSearch(this.gs_);
            this._0A_z.addChild(this.search_);
            this.search_.addEventListener(MarketplaceEvent.SEARCH, this.searchRebuild);
            this.search_.addEventListener(Event.CHANGE, this.checkScroll);
        } else {
            removeChild(this.titleText_);
            this.titleText_ = null;
            removeChild(this._dL_);
            this._dL_ = null;
            removeChild(this._017);
            this._017 = null;
            this._0A_z = null;
            removeChild(this.allButton_);
            this.allButton_ = null;
            removeChild(this.mineButton_);
            this.mineButton_ = null;
            removeChild(this.searchButton_);
            this.searchButton_ = null;
            if(this._E_k != null && contains(this._E_k)) {
                removeChild(this._E_k);
                this._E_k = null;
            }

            /*this._j7 = new BaseSimpleText(22, 0xB3B3B3, false, 0, 0);
            this._j7.setBold(true);
            this._j7.text = "Loading...";
            this._E_k.useTextDimensions();
            this._j7.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
            this._j7.x = ((800 / 2) - (this._j7.width / 2));
            this._j7.y = ((600 / 2) - (this._j7.height / 2));
            addChild(this._j7);*/
            this.loadingText_ = new TextFieldDisplayConcrete().setSize(22).setColor(0xB3B3B3);
            this.loadingText_.setBold(true);
            this.loadingText_.setStringBuilder(new LineBuilder().setParams(TextKey.LOADING_TEXT));
            this.loadingText_.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
            this.loadingText_.setAutoSize(TextFieldAutoSize.CENTER).setVerticalAlign(TextFieldDisplayConcrete.MIDDLE);
            this.loadingText_.x = (800 / 2);
            this.loadingText_.y = 550;
            addChild(this.loadingText_);
            var _local_4:Account = StaticInjectorContext.getInjector().getInstance(Account);
            var _local_5:Object = {
                "num": this.num_,
                "offset": this.offset_,
                "filter": this.category_
            };
            MoreObjectUtil.addToObject(_local_5, _local_4.getCredentials());
            this.listClient_ = StaticInjectorContext.getInjector().getInstance(AppEngineClient);
            this.listClient_.setMaxRetries(2);
            this.listClient_.complete.addOnce(this.onComplete);
            this.listClient_.sendRequest("/market/list", _local_5);
        }
    }

    public function searchRebuild(e:MarketplaceEvent):void {
        this.category_ = "searched";

        removeChild(this.titleText_);
        this.titleText_ = null;
        removeChild(this._dL_);
        this._dL_ = null;
        removeChild(this._017);
        this._017 = null;
        this._0A_z = null;
        this.search_ = null;
        removeChild(this.allButton_);
        this.allButton_ = null;
        removeChild(this.mineButton_);
        this.mineButton_ = null;
        removeChild(this.searchButton_);
        this.searchButton_ = null;
        if(this._E_k != null && contains(this._E_k)) {
            removeChild(this._E_k);
            this._E_k = null;
        }
        this.loadingText_ = new TextFieldDisplayConcrete().setSize(22).setColor(0xB3B3B3);
        this.loadingText_.setBold(true);
        this.loadingText_.setStringBuilder(new LineBuilder().setParams(TextKey.LOADING_TEXT));
        this.loadingText_.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
        this.loadingText_.setAutoSize(TextFieldAutoSize.CENTER).setVerticalAlign(TextFieldDisplayConcrete.MIDDLE);
        this.loadingText_.x = (800 / 2);
        this.loadingText_.y = 550;
        addChild(this.loadingText_);
        var _local_5:Object = {
            "num": this.num_,
            "offset": this.offset_,
            "filter": this.category_,
            "offerItems": e.offerItems_.join(),
            "offerData": JSON.stringify({datas:e.offerData_}),
            "requestItems": e.requestItems_.join(),
            "requestData": JSON.stringify({datas:e.requestData_})
        };
        var _local_4:Account = StaticInjectorContext.getInjector().getInstance(Account);
        MoreObjectUtil.addToObject(_local_5, _local_4.getCredentials());
        this.listClient_ = StaticInjectorContext.getInjector().getInstance(AppEngineClient);
        this.listClient_.setMaxRetries(2);
        this.listClient_.complete.addOnce(this.onComplete);
        this.listClient_.sendRequest("/market/list", _local_5);
    }

    private function buildCategoryButtons(){
        this.allButton_ = new ClickableTextBase(18, false, "");
        this.allButton_.text_.setBold(true);
        this.allButton_.text_.setStringBuilder(new LineBuilder().setParams("All Trades"));
        this.allButton_.text_.setColor(0xB3B3B3);
        //this.allButton_.text_.updateMetrics();
        this.allButton_.x = 18;
        this.allButton_.y = 75;
        this.allButton_.addEventListener(MouseEvent.CLICK, this.categoryAll);
        addChild(this.allButton_);
        this.mineButton_ = new ClickableTextBase(18, false, "");
        this.mineButton_.text_.setBold(true);
        this.mineButton_.text_.setStringBuilder(new LineBuilder().setParams("My Trades"));
        this.mineButton_.text_.setColor(0xB3B3B3);
        //this.mineButton_.text_.updateMetrics();
        this.mineButton_.x = 18 + 150;
        this.mineButton_.y = 75;
        this.mineButton_.addEventListener(MouseEvent.CLICK, this.categoryMine);
        addChild(this.mineButton_);
        this.searchButton_ = new ClickableTextBase(18, false, "");
        this.searchButton_.text_.setBold(true);
        this.searchButton_.text_.setStringBuilder(new LineBuilder().setParams("Search"));
        this.searchButton_.text_.setColor(0xB3B3B3);
        //this.searchButton_.text_.updateMetrics();
        this.searchButton_.x = 18 + 150 + 150;
        this.searchButton_.y = 75;
        this.searchButton_.addEventListener(MouseEvent.CLICK, this.categorySearch);
        addChild(this.searchButton_);
    }

    private function onComplete(_arg_1:Boolean, _arg_2:*):void {
        if (_arg_1) {
            this.onGenericData(_arg_2);
        }
        else {
            this.onTextError(_arg_2);
        }
    }

    private function onGenericData(_arg_1:String):void {
        this.build(XML(_arg_1));
    }

    private function onTextError(_arg_1:String):void {
    }

    private function _A_E_(_arg1:Event):void {
        this._0A_z.y = (-(this._E_k.pos()) * ((this._0A_z.height + 12) - 400));
    }

    private function _B_E_(_arg1:Event):void {
        this._0A_z.y = (-(this._E_k.pos()) * ((this._0A_z.height + 70) - 400));
    }

    private function checkScroll(event:Event):void {
        if(this._E_k != null && this.contains(this._E_k)) {
            this.removeChild(this._E_k);
            this._E_k = null;
        }
        if (this._0A_z.height > 400) {
            this._E_k = new Scrollbar(16, 400);
            this._E_k.x = ((800 - this._E_k.width) - 4);
            this._E_k.y = 110;
            this._E_k.setIndicatorSize(400, this._0A_z.height + 70);
            this._E_k.addEventListener(Event.CHANGE, this._B_E_);
            addChild(this._E_k);
        }
    }

    private function categoryAll(event:MouseEvent):void {
        if(this.category_ != "all") {
            this.category_ = "all";
            this.rebuild();
        }
    }

    private function categoryMine(event:MouseEvent):void {
        if(this.category_ != "mine") {
            this.category_ = "mine";
            this.rebuild();
        }
    }

    private function categorySearch(event:MouseEvent):void {
        if(this.category_ != "search") {
            this.category_ = "search";
            this.rebuild();
        }
    }
}
}//package _sP_

