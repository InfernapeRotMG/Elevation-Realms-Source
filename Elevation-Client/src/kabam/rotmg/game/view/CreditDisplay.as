package kabam.rotmg.game.view {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.util.FameUtil;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.assembleegameclient.util.TimeUtil;
import com.company.util.AssetLibrary;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;

import kabam.rotmg.assets.services.IconFactory;
import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.fortune.model.FortuneInfo;
import kabam.rotmg.fortune.services.FortuneModel;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
import kabam.rotmg.ui.view.SignalWaiter;

import org.osflash.signals.Signal;

public class CreditDisplay extends Sprite {

    private static const FONT_SIZE:int = 18;
    public static const waiter:SignalWaiter = new SignalWaiter();

    private var creditsText_:TextFieldDisplayConcrete;
    private var silverText_:TextFieldDisplayConcrete;
    private var bronzeText_:TextFieldDisplayConcrete;
    private var fameText_:TextFieldDisplayConcrete;
    private var coinIcon_:Bitmap;
    private var silverIcon_:Bitmap;
    private var bronzeIcon_:Bitmap;
    private var fameIcon_:Bitmap;
    private var credits_:int = -1;
    private var silver_:int = -1;
    private var bronze_:int = -1;
    private var fame_:int = -1;
    private var gs:GameSprite;
    public var openAccountDialog:Signal;

    public function CreditDisplay(_arg_1:GameSprite = null, _arg2:Boolean = true) {
        this.openAccountDialog = new Signal();
        super();
        this.gs = _arg_1;
        this.creditsText_ = this.makeTextField();
        waiter.push(this.creditsText_.textChanged);
        addChild(this.creditsText_);
        var coinBD:BitmapData = AssetLibrary.getImageFromSet("lofiObj3", 225);
        coinBD = TextureRedrawer.redraw(coinBD, 40, true, 0, 0);
        this.coinIcon_ = new Bitmap(coinBD);
        addChild(this.coinIcon_);

        this.silverText_ = this.makeTextField();
        waiter.push(this.silverText_.textChanged);
        addChild(this.silverText_);
        var silverBD:BitmapData = AssetLibrary.getImageFromSet("lofiObj3", 1381);
        silverBD = TextureRedrawer.redraw(silverBD, 40, true, 0, 0);
        this.silverIcon_ = new Bitmap(silverBD);
        addChild(this.silverIcon_);

        this.bronzeText_ = this.makeTextField();
        waiter.push(this.bronzeText_.textChanged);
        addChild(this.bronzeText_);
        var bronzeBD:BitmapData = AssetLibrary.getImageFromSet("lofiObj3", 1382);
        bronzeBD = TextureRedrawer.redraw(bronzeBD, 40, true, 0, 0);
        this.bronzeIcon_ = new Bitmap(bronzeBD);
        addChild(this.bronzeIcon_);

        this.fameText_ = this.makeTextField();
        waiter.push(this.fameText_.textChanged);
        this.fameText_.visible = _arg2;
        addChild(this.fameText_);
        this.fameIcon_ = new Bitmap(FameUtil.getFameIcon());
        this.fameIcon_.visible = _arg2;
        addChild(this.fameIcon_);

        this.draw(0, 0);
        mouseEnabled = true;
        doubleClickEnabled = true;
        addEventListener(MouseEvent.DOUBLE_CLICK, this.onDoubleClick, false, 0, true);
        waiter.complete.add(this.onAlignHorizontal);
    }

    private function onAlignHorizontal():void {
        this.fameIcon_.x = -(this.fameIcon_.width);
        this.fameText_.x = ((this.fameIcon_.x - this.fameText_.width) + 8);
        this.fameText_.y = ((this.fameIcon_.height / 2) - (this.fameText_.height / 2));

        this.silverIcon_.x = (this.fameText_.x - this.silverIcon_.width);
        this.silverText_.x = ((this.silverIcon_.x - this.silverText_.width) + 8);
        this.silverText_.y = ((this.silverIcon_.height / 2) - (this.silverText_.height / 2));

        this.bronzeIcon_.x = (this.silverText_.x - this.bronzeIcon_.width);
        this.bronzeText_.x = ((this.bronzeIcon_.x - this.bronzeText_.width) + 8);
        this.bronzeText_.y = ((this.bronzeIcon_.height / 2) - (this.bronzeText_.height / 2));

        this.coinIcon_.x = (this.bronzeText_.x - this.coinIcon_.width);
        this.creditsText_.x = ((this.coinIcon_.x - this.creditsText_.width) + 8);
        this.creditsText_.y = ((this.coinIcon_.height / 2) - (this.creditsText_.height / 2));
    }

    private function onDoubleClick(_arg_1:MouseEvent):void {
        if (((((!(this.gs)) || (this.gs.evalIsNotInCombatMapArea()))) || ((Parameters.data_.clickForGold == true)))) {
            this.openAccountDialog.dispatch();
        }
    }

    public function makeTextField(_arg_1:uint = 0xFFFFFF):TextFieldDisplayConcrete {
        var _local_2:TextFieldDisplayConcrete = new TextFieldDisplayConcrete().setSize(FONT_SIZE).setColor(_arg_1).setTextHeight(16);
        _local_2.filters = [new DropShadowFilter(0, 0, 0, 1, 4, 4, 2)];
        return (_local_2);
    }

    private function setGoldSilverBronze(_arg_1:int):void{
        this.credits_ = _arg_1 / 10000;
        this.creditsText_.setStringBuilder(new StaticStringBuilder(this.credits_.toString()));
        var holder:int = _arg_1 % 10000;
        this.silver_ = holder / 100;
        this.silverText_.setStringBuilder(new StaticStringBuilder(this.silver_.toString()));
        this.bronze_ = holder % 100;
        this.bronzeText_.setStringBuilder(new StaticStringBuilder(this.bronze_.toString()));
    }

    public function draw(_arg_1:int, _arg_2:int):void {
        if (_arg_1 == this.credits_ && _arg_2 == this.fame_) {
            return;
        }

        setGoldSilverBronze(_arg_1);

        this.fame_ = _arg_2;
        this.fameText_.setStringBuilder(new StaticStringBuilder(this.fame_.toString()));

        if (waiter.isEmpty()) {
            this.onAlignHorizontal();
        }
    }
}
}//package kabam.rotmg.game.view
