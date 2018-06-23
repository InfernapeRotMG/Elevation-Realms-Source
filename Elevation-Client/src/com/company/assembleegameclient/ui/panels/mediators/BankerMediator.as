package com.company.assembleegameclient.ui.panels.mediators {
import com.company.assembleegameclient.account.ui.BankerFrame;
import com.company.assembleegameclient.account.ui.BankerFrame;
import com.company.assembleegameclient.ui.panels.BankerPanel;

import kabam.rotmg.account.core.view.RegisterPromptDialog;
import kabam.rotmg.account.web.view.WebLoginDialog;
import kabam.rotmg.dialogs.control.CloseDialogsSignal;
import kabam.rotmg.dialogs.control.OpenDialogNoModalSignal;
import kabam.rotmg.dialogs.control.OpenDialogSignal;
import kabam.rotmg.game.model.GameModel;
import kabam.rotmg.ui.model.HUDModel;
import robotlegs.bender.bundles.mvcs.Mediator;

public class BankerMediator extends Mediator {

    [Inject]
    public var view:BankerPanel;
    [Inject]
    public var openDialog:OpenDialogNoModalSignal;
    [Inject]
    public var hudModel:HUDModel;
    [Inject]
    public var gameModel:GameModel;

    override public function initialize():void {
        this.view.triggered.add(this.onDispatchBank);
    }

    override public function destroy():void {
        this.view.triggered.remove(this.onDispatchBank);
    }

    private function onDispatchBank():void {
        var bankerFrame:BankerFrame = new BankerFrame(this.hudModel.gameSprite);
        bankerFrame.setBankData(gameModel.player.credits_, gameModel.player.fame_);
        this.openDialog.dispatch(bankerFrame);
    }
}
}//package com.company.assembleegameclient.ui.panels.mediators
