@echo off

REM git remote add local http://git.biodeep.cn/biodeep/toolkits/KEGG-molDraw.git

git pull gitee HEAD
git pull local HEAD

git push gitee HEAD
git push local HEAD


echo synchronization of this code repository job done!