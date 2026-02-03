@echo off
chcp 65001 >nul
echo ========================================
echo   Smart Library - أدوات البناء والتشغيل
echo ========================================
echo.

echo [1/4] جاري التحقق من متطلبات .NET Framework...
reg query "HKLM\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" /v Release >nul 2>&1
if %errorlevel% equ 0 (
    echo ✅ .NET Framework 4.7.2 موجود
) else (
    echo ⚠️  يرجى تثبيت .NET Framework 4.7.2 أو أحدث
    goto :end
)

echo.
echo [2/4] جاري تنظيف البناء السابق...
if exist "bin\Release" rmdir /s /q "bin\Release"
if exist "obj\Release" rmdir /s /q "obj\Release"
echo ✅ تم التنظيف

echo.
echo [3/4] جاري بناء المشروع...
echo    (إذا فشل البناء، يرجى فتح الحل في Visual Studio)
echo.

REM التحقق من وجود Visual Studio
where devenv.com >nul 2>&1
if %errorlevel% equ 0 (
    echo ✅ تم العثور على Visual Studio
    devenv.com smartLibraryForC#.sln /build Release
    if %errorlevel% equ 0 (
        echo ✅ تم البناء بنجاح!
    ) else (
        echo ❌ فشل البناء
        goto :end
    )
) else (
    echo ℹ️  Visual Studio غير موجود، جاري استخدام MSBuild...
    
    where msbuild.exe >nul 2>&1
    if %errorlevel% equ 0 (
        msbuild.exe smartLibraryForC#.csproj /p:Configuration=Release
        if %errorlevel% equ 0 (
            echo ✅ تم البناء بنجاح!
        ) else (
            echo ⚠️  فشل البناء، يرجى فتح الحل في Visual Studio
        )
    ) else (
        echo ℹ️  MSBuild غير موجود، جاري إنشاء تعليمات...
        echo.
        echo ========================================
        echo   تعليمات البناء اليدوي
        echo ========================================
        echo.
        echo 1. افتح ملف "smartLibraryForC#.sln" في Visual Studio
        echo 2. تأكد من تثبيت الحزم التالية من NuGet:
        echo    - Microsoft.Data.Sqlite (8.0.0)
        echo    - System.Text.Json (8.0.0)
        echo    - Newtonsoft.Json (13.0.3)
        echo    - PdfiumViewer (2.13.0.0)
        echo 3. اختر "Release" من قائمة التكوين
        echo 4. اضغط Ctrl+Shift+B للبناء
        echo 5. اضغط F5 للتشغيل
        echo.
    )
)

echo.
echo [4/4] جاري التحقق من ملفات التشغيل...
set "files_ok=true"

if not exist "bin\Release\SmartLibrary.exe" (
    echo ⚠️  ملف SmartLibrary.exe غير موجود
    set "files_ok=false"
)

if not exist "bin\Release\SmartLibrary.db" (
    echo ℹ️  ملف قاعدة البيانات سيتم إنشاؤه تلقائياً
)

if not exist "bin\Release\Books" (
    echo ℹ️  مجلد Books سيتم إنشاؤه تلقائياً
)

if "%files_ok%"=="true" (
    echo ✅ جميع الملفات المطلوبة موجودة
    echo.
    echo ========================================
    echo   يمكنك الآن تشغيل التطبيق!
    echo ========================================
    echo.
    echo لتشغيل التطبيق، قم بـ:
    echo 1. فتح المجلد "bin\Release"
    echo 2. تشغيل ملف "SmartLibrary.exe"
    echo.
)

:end
echo.
pause
