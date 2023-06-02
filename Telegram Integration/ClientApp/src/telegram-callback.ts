interface TelegramUser {
  id: number;
  firstName: string;
  lastName: string;
  username: string;
  authDate: number;
  hash: string;
}

export function loginViaTelegram(user: TelegramUser) {
  console.log(user);
}
