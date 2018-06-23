package com.company.assembleegameclient.ui.panels {
import com.company.assembleegameclient.account.ui.BankerFrame;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.parameters.Parameters;
import flash.events.Event;
import flash.events.KeyboardEvent;
import flash.events.MouseEvent;

import org.osflash.signals.Signal;

public class BankerPanel extends ButtonPanel {

    public var triggered:Signal;

    public function BankerPanel(_arg_1:GameSprite) {
        super(_arg_1, "Banker", "Access");

        this.triggered = new Signal();
        addEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
        addEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
    }

    private function onAddedToStage(_arg_1:Event):void {
        stage.addEventListener(KeyboardEvent.KEY_DOWN, this.onKeyDown);
    }

    private function onRemovedFromStage(_arg_1:Event):void {
        stage.removeEventListener(KeyboardEvent.KEY_DOWN, this.onKeyDown);
    }

    override protected function onButtonClick(_arg_1:MouseEvent):void {
        this.triggered.dispatch();
    }

    private function onKeyDown(_arg_1:KeyboardEvent):void {
        if ((((_arg_1.keyCode == Parameters.data_.interact)) && ((stage.focus == null)))) {
            this.triggered.dispatch();
        }
    }
}
}//package kabam.rotmg.game.view
