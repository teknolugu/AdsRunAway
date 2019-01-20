# AdsRunAway
AdsRunAway is a software for block ads when you surfing in the internet. it is inspired by AdAway for Android
# How it Works?
AdsRunAway using hosts file for blocking ads server. The hosts file can be found at **%WINDIR% \System32\drivers\etc** 
1. First AdsRunAway will ask you to disable DNSClient service on windows, because it will makes your internet slow if you use hosts file that has very many entries
2. And then AdsRunAway will downloads all the hosts source file from list of urls stored on app
3. If the download has finished AdsRunAway will merge all of them, and will replace the original hosts file on the system
