package com.company.assembleegameclient.account.ui {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.DeprecatedTextButton;

import flash.events.MouseEvent;

import kabam.rotmg.game.view.CreditDisplay;
import kabam.rotmg.pets.util.PetsViewAssetFactory;
import kabam.rotmg.pets.view.components.DialogCloseButton;

import org.osflash.signals.Signal;

public class BankerFrame extends Frame {

    private var gs_:GameSprite;

    public const close:Signal = new Signal();

    protected var withdrawButton_:DeprecatedTextButton;
    protected var depositButton_:DeprecatedTextButton;
    protected var creditDisplay_:CreditDisplay;
    private var closeButton_:DialogCloseButton;

    public function BankerFrame(gs:GameSprite) {
        super("Bank", "", "");
        this.gs_ = gs;

        this.w_ = 352;
        this.h_ = 288 / 2;

        this.creditDisplay_ = new  CreditDisplay(this.gs_, false);
        addChild(this.creditDisplay_);

        this.withdrawButton_ = new DeprecatedTextButton(16, "Withdraw");
        this.withdrawButton_.addEventListener(MouseEvent.CLICK, this.onWithdrawClick);
        this.withdrawButton_.textChanged.addOnce(this.alignUI);
        addChild(this.withdrawButton_);

        this.depositButton_ = new DeprecatedTextButton(16, "Deposit");
        this.depositButton_.addEventListener(MouseEvent.CLICK, this.onDepositClick);
        this.depositButton_.textChanged.addOnce(this.alignUI);
        addChild(this.depositButton_);

        this.closeButton_ = PetsViewAssetFactory.returnCloseButton(this.w_);
        addChild(this.closeButton_);
    }

    public function onWithdrawClick(event:MouseEvent):void {
        //todo withdraw frame
    }
    public function onDepositClick(event:MouseEvent):void {
//        this.gs_.gsc_.sendBankDeposit();
        this.closeButton_.clicked.dispatch();
    }

    private function alignUI():void {
        this.creditDisplay_.x = (this.w_ - (this.creditDisplay_.width / 2) +( this.creditDisplay_.width / 4) - 7);
        this.creditDisplay_.y = ((this.h_ / 3) - (this.creditDisplay_.height / 2));

        this.withdrawButton_.x = (((this.w_ / 4) * 1) - (this.depositButton_.width / 2));
        this.withdrawButton_.y = ((this.h_ - this.depositButton_.height) - 32);
        this.depositButton_.x = (((this.w_ / 4) * 3) - (this.withdrawButton_.width / 2));
        this.depositButton_.y = ((this.h_ - this.withdrawButton_.height) - 32);

        this.closeButton_.x -= 4;
        this.closeButton_.y -= 2;
    }

    public function setBankData(_arg1:int, _arg2:int){
        this.creditDisplay_.draw(_arg1, _arg2);
    }
}
}//package com.company.assembleegameclient.account.ui
