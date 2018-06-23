/**
 * Created by club5_000 on 9/21/2014.
 */
package com.company.assembleegameclient.ui.guild {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.ui.Scrollbar;
import com.company.ui.BaseSimpleText;

    import flash.display.Bitmap;

    import flash.display.Graphics;

    import flash.display.Shape;

    import flash.display.Sprite;
    import flash.events.Event;
    import flash.events.MouseEvent;
    import flash.filters.DropShadowFilter;

    import spark.primitives.Graphic;

    public class MarketplaceSearch extends Sprite {

        public function MarketplaceSearch(_arg1:AGameSprite) {
            super();
            this.gs_ = _arg1;
            this.build();
        }
        private var gs_:AGameSprite;
        private var _P_V_:BaseSimpleText;
        private var _dL_:Shape;
        private var _017:Sprite;
        private var _0A_z:Sprite;
        private var _E_k:Scrollbar;
        private var offer_:MarketplaceItemSelect;
        private var arrow_:Bitmap;
        private var request_:MarketplaceItemSelect;
        private var searchButton_:DeprecatedTextButton;

        public function build():void {
            this.offer_ = new MarketplaceItemSelect(this.gs_);
            this.offer_.x = 34;
            this.offer_.y = 58;
            addChild(this.offer_);
            this.arrow_ = new TradeArrowEmbed();
            this.arrow_.x = this.offer_.x + this.offer_.width + 10;
            this.arrow_.y = this.offer_.y + 50 - (this.arrow_.height / 2);
            addChild(this.arrow_);
            this.request_ = new MarketplaceItemSelect(this.gs_);
            this.request_.x = this.arrow_.x + this.arrow_.width + 18;
            this.request_.y = 58;
            addChild(this.request_);
            this.searchButton_ = new DeprecatedTextButton(12, "Search");
            this.searchButton_.x = (((3 * MemberListLine.WIDTH) / 3.25) - (this.searchButton_.width / 2));
            this.searchButton_.y = this.arrow_.y + (this.arrow_.height / 2) - (this.searchButton_.height / 2);
            addChild(this.searchButton_);
            this.searchButton_.addEventListener(MouseEvent.CLICK, this.createTrade);
        }

        private function _A_E_(_arg1:Event):void {
            this._0A_z.y = (-(this._E_k.pos()) * (this._0A_z.height - 400));
        }

        private function checkScroll(event:Event):void {
            if(this._E_k != null && this.contains(this._E_k)) {
                this.removeChild(this._E_k);
                this._E_k = null;
            }
            if (this._0A_z.height > 400) {
                this._E_k = new Scrollbar(16, 400);
                this._E_k.x = ((800 - this._E_k.width) - 4);
                this._E_k.y = 108;
                this._E_k.setIndicatorSize(400, this._0A_z.height);
                this._E_k.addEventListener(Event.CHANGE, this._A_E_);
                addChild(this._E_k);
            }
        }

        private function createTrade(event:MouseEvent):void {
            this.dispatchEvent(MarketplaceEvent.Search(this.offer_, this.request_));
        }
    }
}
