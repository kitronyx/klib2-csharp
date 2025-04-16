# KLib2-CSharp

KLib2-CSharp is a WinForms-based C# sample application developed for real-time communication with devices such as **ForceLAB2** and **Snowforce3**.  
This example demonstrates how to use the KLib2 API to establish a device connection and visually display incoming data in a GUI environment.

> This code is provided for demonstration purposes. Actual performance may vary depending on your system environment.

---

## Key Features

- Device connection and real-time data acquisition via KLib2 API
- 2D label-based visualization of received data
- Dynamic UI resizing according to sensor grid size
- Lightweight and standalone implementation using Windows Forms

---

## Development Environment

- Language: C#
- Framework: .NET Framework 4.6.1 or higher
- Target Devices: ForceLAB2, Snowforce3

---

## Quick Start

### 1. Add Reference

Include `KLib2_CSharp.dll` in your project references.

### 2. Run the App

Include `Form1.cs` in your project and build the application.

### 3. Connect to Device

Click the `Connect` button to begin device communication. Data will be displayed in real-time.

### 4. Auto-Disconnect

If communication fails, the app will auto-disconnect and alert the user.

---

## Code Overview

### Device Connection

```csharp
if (klib.Start())
{
    connet = true;
    thread = new Thread(new ThreadStart(workingthread));
    thread.Start();
    button1.Text = "Disconnect";
}
```

### Dynamic UI

```csharp
labels = new Label[nCol][];
for (int i = 0; i < nCol; ++i)
{
    labels[i] = new Label[nRow];
    for (int j = 0; j < nRow; ++j)
    {
        labels[i][j] = new Label();
        labels[i][j].Text = "0";
        labels[i][j].Location = new Point(i * 20, j * 20);
        Controls.Add(labels[i][j]);
    }
}
```

### Live Data Update

```csharp
if (labels[i][j].Text != data[j * nCol + i].ToString())
{
    labels[i][j].Text = data[j * nCol + i].ToString();
}
```

### Safe Shutdown

```csharp
private void Form1_FormClosed(object sender, FormClosedEventArgs e)
{
    if (thread != null) thread.Abort();
}
```

---

## Class & Function Summary

| Element             | Type      | Description |
|---------------------|-----------|-------------|
| `Form1`             | Class     | Main Windows Form handling device connection and data display |
| `Form1()`           | Constructor | Initializes UI and button state |
| `button1_Click()`   | Method    | Toggles connection and starts/stops data thread |
| `workingthread()`   | Method    | Continuously receives data from the device and updates labels |
| `FormResize()`      | Method    | Adjusts form size dynamically based on data grid dimensions |
| `labelsdatachange()`| Method    | Updates label text with new sensor value |
| `contorolsAdd()`    | Method    | Adds new label control to the form |
| `Form1_FormClosed()`| Method    | Aborts data thread safely when the form is closed |

---

## Contact

For technical support or inquiries,  
please visit **https://www.kitronyx.com/support_request** or contact your representative.

---

# KLib2-CSharp

KLib2-CSharp는 **ForceLAB2** 및 **Snowforce3** 장비와의 실시간 통신을 위해 개발된 WinForms 기반의 C# 샘플 애플리케이션입니다.  
본 예제는 KLib2 API를 활용해 디바이스와 연결을 수행하고, 데이터를 시각적으로 표시하는 GUI 환경을 제공합니다.

> 본 코드는 예제 용도로 제공되며, 실제 성능은 시스템 환경에 따라 달라질 수 있습니다.

---

## 주요 특징

- KLib2 API를 통한 장치 연결 및 실시간 데이터 수신
- 수신된 데이터를 2D 라벨 그리드로 시각화
- 센서 배열 크기에 따른 UI 자동 리사이징
- WinForms 기반의 가벼운 단독 실행 애플리케이션

---

## 개발 환경

- 언어: C#
- 프레임워크: .NET Framework 4.6.1 이상
- 대상 장비: ForceLAB2, Snowforce3

---

## 퀵스타트

### 1. 참조 추가

`KLib2_CSharp.dll`을 프로젝트 참조에 추가합니다.

### 2. 실행

`Form1.cs` 파일을 프로젝트에 포함시키고 애플리케이션을 빌드합니다.

### 3. 장치 연결

`Connect` 버튼을 클릭하면 장치와 연결되고, 데이터가 실시간으로 표시됩니다.

### 4. 자동 종료 처리

연결이 끊기거나 오류 발생 시 자동으로 연결이 해제되고 알림이 표시됩니다.

---

## 코드 개요

### 디바이스 연결

```csharp
if (klib.Start())
{
    connet = true;
    thread = new Thread(new ThreadStart(workingthread));
    thread.Start();
    button1.Text = "Disconnect";
}
```

### UI 자동 생성

```csharp
labels = new Label[nCol][];
for (int i = 0; i < nCol; ++i)
{
    labels[i] = new Label[nRow];
    for (int j = 0; j < nRow; ++j)
    {
        labels[i][j] = new Label();
        labels[i][j].Text = "0";
        labels[i][j].Location = new Point(i * 20, j * 20);
        Controls.Add(labels[i][j]);
    }
}
```

### 실시간 데이터 갱신

```csharp
if (labels[i][j].Text != data[j * nCol + i].ToString())
{
    labels[i][j].Text = data[j * nCol + i].ToString();
}
```

### 안전한 종료 처리

```csharp
private void Form1_FormClosed(object sender, FormClosedEventArgs e)
{
    if (thread != null) thread.Abort();
}
```

---

## Class & Function Summary

| 구성 요소               | 종류     | 설명 |
|--------------------------|----------|------|
| `Form1`                 | 클래스   | 디바이스 연결 및 데이터 표시를 담당하는 메인 폼 |
| `Form1()`               | 생성자   | UI 초기화 및 버튼 상태 설정 |
| `button1_Click()`       | 메서드   | 연결 상태를 전환하고 데이터 수신 스레드 시작/중지 |
| `workingthread()`       | 메서드   | 디바이스에서 데이터를 지속적으로 받아 라벨에 갱신 |
| `FormResize()`          | 메서드   | 데이터 그리드 크기에 따라 폼 크기 자동 조절 |
| `labelsdatachange()`    | 메서드   | 센서 값에 따라 라벨 텍스트 업데이트 |
| `contorolsAdd()`        | 메서드   | 새 라벨 컨트롤을 폼에 추가 |
| `Form1_FormClosed()`    | 메서드   | 폼 종료 시 데이터 수신 스레드를 안전하게 종료 |

---

## 문의

기술 지원 또는 문의는  
**https://www.kitronyx.co.kr/support_request** 를 방문하거나 담당자에게 문의해 주세요.