package com.company.assembleegameclient.objects {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.panels.MarketPanel;
import com.company.assembleegameclient.ui.panels.Panel;
import kabam.rotmg.game.view.MoneyChangerPanel;

public class MarketplaceGround extends GameObject implements IInteractiveObject {

    public function MarketplaceGround(_arg_1:XML) {
        super(_arg_1);
        isInteractive_ = true;
    }

    public function getPanel(_arg_1:GameSprite):Panel {
        return (new MarketPanel(_arg_1));
    }


}
}//package com.company.assembleegameclient.objects
