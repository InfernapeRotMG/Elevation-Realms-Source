package com.company.assembleegameclient.ui.panels {
import com.company.assembleegameclient.ui.panels.*;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.ui.board.GuildBoardWindow;
import com.company.assembleegameclient.ui.guild.MarketplaceContainerUI;
import com.company.assembleegameclient.util.GuildUtil;

import flash.events.MouseEvent;

import kabam.rotmg.text.model.TextKey;

public class MarketPanel extends ButtonPanel {

    public function MarketPanel(_arg_1:GameSprite) {
        super(_arg_1, TextKey.MARKET_PLACE1, TextKey.MARKET_PLACE2);
    }

    override protected function onButtonClick(_arg_1:MouseEvent):void {
        var _local_2:Player = gs_.map.player_;
        if (_local_2 == null) {
            return;
        }
        gs_.mui_.clearInput();
        gs_.addChild(new MarketplaceContainerUI(gs_));
    }


}
}//package com.company.assembleegameclient.ui.panels
