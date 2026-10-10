Github: https://github.com/jacksonchao2007/1151VR-HW2-414262698--
Youtube: https://youtu.be/X-13msTkgxQ

製作及操作流程:
遊戲開始時，Player 出現在 Start。使用 A/D 或方向鍵左右移動，按 Space 跳躍。角色依移動方向翻轉圖片，並受左右邊界限制。碰到 End 後停止並顯示「Victory!」。
Unity 製作：Player 加入 Rigidbody 2D、Collider 2D 和 PlayerMove；End 加入勾選 Is Trigger 的 Collider 2D；地面加入 Collider 2D。將 Start、End 和勝利文字拖入 PlayerMove 對應欄位。Vector2 控制移動，horizontalLimits 陣列設定左右邊界。
