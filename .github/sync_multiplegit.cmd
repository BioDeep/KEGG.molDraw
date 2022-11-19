@echo off

git pull gitee HEAD
git pull local HEAD

git push gitee HEAD
git push local HEAD


echo synchronization of this code repository job done!