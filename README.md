# 📚 SmartLibrary
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-purple?style=flat-square&logo=dotnet)
![Language](https://img.shields.io/badge/Language-C%23-blue?style=flat-square&logo=csharp)
![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)
![Status](https://img.shields.io/badge/Status-Active-success?style=flat-square)

> **A comprehensive desktop library manager and reader built with Windows Forms.**  
> *تطبيق مكتبي ذكي لإدارة المكتبات والقراءة مبني بتقنية Windows Forms.*

<p align="center">
  <a href="#-english"><b>🇬🇧 English</b></a> | <a href="#-arabic"><b>🇸🇦 العربية</b></a>
</p>

---

<div id="english"></div>

## 🇬🇧 Overview
**SmartLibrary** is a feature-rich desktop application built with **C#** and **.NET Framework 4.7.2**. It provides a seamless environment for book enthusiasts to **search**, **download**, and **read** books from multiple global sources directly within the application, eliminating the need for external tools.

It features a modern, responsive user interface with **Dark/Light** themes and unique additions like a real-time **weather widget** to set the perfect reading atmosphere.

## ✨ Key Features
-   **🔍 Intelligent Multi-Source Search**:
    -   Primary integration with **Open Library API**.
    -   Smart fallback system searching **Internet Archive** and **Project Gutenberg** automatically.
-   **📖 Built-in PDF Reader**:
    -   Powered by the robust `PdfiumViewer` engine.
    -   Read instantly without leaving the app.
-   **💾 Local Library Management**:
    -   Save favorites and download books locally.
    -   Track reading history and manage your personal collection via **SQLite**.
-   **🎨 Customizable UI**:
    -   Advanced **ThemeManager** supporting Dark & Light modes.
-   **☁️ Smart Add-ons**:
    -   **WeatherService**: Displays current weather conditions.
    -   **LogManager**: Robust error tracking system.

## 🛠 Tech Stack
| Category | Technology |
|----------|------------|
| **Framework** | .NET Framework 4.7.2 (WinForms) |
| **Language** | C# 10.0 |
| **Database** | SQLite |
| **PDF Engine** | PdfiumViewer |
| **Network** | HttpClient (TLS 1.2+) |
| **Data** | Newtonsoft.Json, System.Text.Json |

## 🚀 Getting Started
1.  **Clone** the repository.
2.  Open `smartLibraryForC#.sln` in **Visual Studio**.
3.  **Restore NuGet Packages** to install dependencies.
4.  Press **Start** to run!

---

<div id="arabic"></div>

## 🇸🇦 نظرة عامة (Arabic)
**SmartLibrary** هو تطبيق سطح مكتب متكامل تم بناؤه باستخدام لغة **C#**، يهدف لتقديم تجربة شاملة لمحبي القراءة. يتيح التطبيق للمستخدمين البحث عن الكتب من مصادر عالمية متعددة، تحميلها، وقراءتها مباشرة من داخل البرنامج دون الحاجة لاستخدام متصفح أو قارئ PDF خارجي.

## 🌟 المميزات الرئيسية
-   **🔍 محرك بحث ذكي ومتعدد المصادر**:
    -   يعتمد على **Open Library** كمصدر أساسي.
    -   نظام **Fallback** ذكي يبحث تلقائياً في **Internet Archive** و **Project Gutenberg** في حال عدم توفر الكتاب.
-   **📖 قارئ PDF مدمج**:
    -   يعتمد على محرك `PdfiumViewer` القوي والسريع.
    -   تجربة قراءة سلسة داخل التطبيق.
-   **💾 إدارة المكتبة المحلية**:
    -   حفظ الكتب المفضلة وسجل القراءة.
    -   تخزين البيانات محلياً باستخدام قاعدة بيانات **SQLite** خفيفة وسريعة.
-   **🎨 واجهة مستخدم قابلة للتخصيص**:
    -   دعم كامل للوضع الليلي (**Dark Mode**) والوضع النهاري.
-   **☁️ خدمات إضافية**:
    -   **خدمة الطقس**: لعرض حالة الطقس الحالية وتهيئة جو القراءة.

## 🛠 التقنيات المستخدمة
| الفئة | التقنية |
|-------|---------|
| **إطار العمل** | .NET Framework 4.7.2 |
| **اللغة** | C# 10.0 |
| **قواعد البيانات** | SQLite |
| **قارئ الكتب** | PdfiumViewer |
| **الشبكة** | HttpClient |

## 🚀 كيفية التشغيل
1.  تأكد من وجود **Visual Studio**.
2.  افتح ملف الحل `smartLibraryForC#.sln`.
3.  قم بعمل **Restore NuGet Packages** لتحميل المكتبات المطلوبة.
4.  اضغط **Start** واستمتع بالقراءة!

---
<p align="center">
  <sub>Developed with ❤️ using C#</sub>
</p>
