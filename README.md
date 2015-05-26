
# Windows Application Version Switcher

Windows 用のアプリケーションチェンジャーです。

## 更新履歴

|更新日時 | バージョン | 内容 |
|----------|-------------|------|
| 2015/05/26 | v0.0.1 | とりあえず個人的に使えるのでソース公開 |

## 概要

Windows Application Version Switcher は、最低限の設定で複数のバージョンのコマンドを実行するための仕組みです。
パスの通った場所に実行コマンドと実行コマンドが読み込むファイルを配置することにより、実際に実行するコマンドを簡単に切換することができます。

## 準備

### 実行環境

.NET Framework 4.5 以上がインストールされていること

### 実行ファイルの配置

WinAppVersion.exe をパスの通っている場所に呼び出したいファイル名で配置してください。

コマンド例:
<pre><code>copy WinAppVersion.exe c:\bin\java.exe</code></pre>

### 設定ファイルの配置

各バージョンごとの設定を記述したファイルを実行コマンドと同じ場所に配置します。ファイル名のベース名も同じものにします。

コマンド例:
<pre><code>copy WinAppVersion.json c:\bin\java.json</code></pre>

### 設定ファイルの修正
 
 <pre><code>{
    "default_version": "1.7",
    "version_commands": {
        "1.7": {
            "settings": {
                "JAVA_HOME":  "C:\\Program Files\\Java\\jdk1.7.0_60",
                "PATH": "%JAVA_HOME%\\bin;%PATH%"
            },
            "command": "C:\\Program Files\\Java\\jdk1.7.0_60\\bin\\java.exe"
        },
        "1.8": {
            "settings": {
                "JAVA_HOME":  "C:\\Program Files\\Java\\jdk1.8.0_45",
                "PATH": "%JAVA_HOME%\\bin;%PATH%"
            },
            "command": "C:\\Program Files\\Java\\jdk1.8.0_45\\bin\\java.exe"
        }
    }
}</code></pre>


|要素|説明|
|-----|----|
|default_version| バージョンファイルがない場合に使われるバージョン|
|version_commands|バージョンごとのコマンド設定|
|>"string"|バージョン番号|
|>>settings|コマンドが実行される前に設定される環境変数(キーが KEY の場合 %KEY% が値に入っていた場合は、設定前の値を %KEY% に埋め込みます。既存の値の前後に値をつかしたい場合など用ようです。)|
|>>command|実際に実行されるコマンドのパス|


### バージョンファイルの配置

デフォルトバージョン以外のコマンドを実行する場合に、実行時カレントフォルダにバージョンファイルを配置することによって、実行するバージョンを変更することができます。
名称は、.ベース名-version となります。

コマンド例:
<pre><code>copy WinAppVersion-version c:\Users\unok\git\java_project\.java-version</code></pre>

内容例:
<pre><code>1.8</code></pre>

この状態で、c:\Users\unok\git\java_project で java とうつと 1.8 のバージョンで設定した java が実行されます。


## TODO

久しぶりに数日でつくたアプリケーションなので、不備などいっぱいあるとおもいますので、PR お待ちしております。

- バリデーション強化
- エラーメッセージ強化

## 免責

本アプリケーションは、実験用での利用を想定しており、利用することによって生ずるいかなる損害に対しても一切責任を負いません。

## ライセンス

MIT ライセンス