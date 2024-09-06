# Telegram Notification Bot for Email Alerts

This C# console application allows users to get notified on Telegram when new emails are received through a POP3 email server.

## Features

- Automatically monitors a IMAP email server for new messages.
- Sends a Telegram notification when a new email is received.
- Requires the user's Telegram Bot Token and Chat ID.

## Prerequisites

- .NET SDK 6.0 or higher
- A Telegram Bot created through [BotFather](https://core.telegram.org/bots#botfather)
- IMAP Email Server credentials (Username, Password)

## Setup Instructions

### 1. Create a Telegram Bot

1. Open Telegram and search for [BotFather](https://core.telegram.org/bots#botfather).
2. Start a chat with BotFather and create a new bot by typing `/newbot`.
3. Follow the instructions to give your bot a name and username.
4. After the bot is created, you will receive a **Telegram Token** from BotFather. Save this token for later use in the application.

### 2. Get Your Telegram Chat ID

1. Open Telegram and search for [userinfobot](https://telegram.me/userinfobot).
2. Start a chat with the bot and type `/start`.
3. The bot will return your **Chat ID**. Save this value for later use in the application.

### 3. Prepare Your POP3 Email Server Credentials

- You will need the following information from your email provider:
  - **Email address**: Your full email address (e.g., your_email@example.com)
  - **Password**: Your email account password
  - **POP3 Server address**: The address of the POP3 email server (e.g., pop3.your-email-provider.com)
  - **Port number**: Usually 995 for POP3 with SSL

### 4. Clone the Repository

1. Open a terminal or command prompt.
2. Run the following command to clone this repository to your local machine:

   ```bash
   git clone https://github.com/samogii/TelegramEmailNotificationBot.git
