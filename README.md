# 五子棋
## 規則介紹
若黑棋或白棋一方五個棋子連成線就結束遊戲。
## 規則判斷
開始對局後黑白棋輪流擺放棋子
```
//將位置offset存至陣列中並檢查連線
GamingModel.Step(EStoneType type, int offset)
```
而是否連線則根據四個斜度進行坢定
```
slopes = { 1, 14, 15, 16 }
line = { 0, 0, 0, 0, 1, 0, 0, 0, 0 }

for (var i = 1; i <= 4; i++)
{
   //跟去正反方項檢查棋子並存至 line 中
   (line[4 - i], line[4 + i]) = (Check(-slope), Check(slope))
}

//最後在判定line中是否有連續5個棋子
var connect = CheckLine(line) >= 5
```
