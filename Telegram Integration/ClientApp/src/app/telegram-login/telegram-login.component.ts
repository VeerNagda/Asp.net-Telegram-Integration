import {Component} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {HttpClient, HttpParams} from "@angular/common/http";

@Component({
  selector: 'app-telegram-login',
  templateUrl: './telegram-login.component.html',
  styleUrls: ['./telegram-login.component.css']
})
export class TelegramLoginComponent {
  eventForm: any;
  public telegram: Telegram[] = [];

  constructor(private http: HttpClient, private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.eventForm = this.fb.group({
      otp: ['', Validators.required],
      password: ['', Validators.required],
      phoneNumber: ['', Validators.required],
    });
  }


  onsubmitNumber() {
    const eventData = {...this.eventForm.value};
    const params = new HttpParams().set('info', eventData.phoneNumber);
    this.http.get<Telegram[]>("https://localhost:44408/telegramintegration/login", {params}).subscribe(result => {
      this.telegram = result;
      if (this.telegram.length > 0 && this.telegram[0].message && this.telegram[0].message.includes("PHONE_NUMBER_INVALID"))
        alert("Please Enter a valid Phone Number");
      else if (this.telegram[0].message.includes("Login Successful"))
        alert("Logged in using previous token")

    }, error => console.error(error));
  }

  onsubmitOtp() {
    const eventData = {...this.eventForm.value};
    const params = new HttpParams().set('info', eventData.otp);
    this.http.get<Telegram[]>("https://localhost:44408/telegramintegration/login", {params}).subscribe(result => {
      this.telegram = result;
      if (this.telegram.length > 0 && this.telegram[0].message && this.telegram[0].message.includes("PHONE_CODE_INVALID" || "Wrong verification code!"))
        alert("Please Enter a valid OTP");
      else if (this.telegram[0].message.includes("Login Successful"))
        alert("Login Successful")

    }, error => console.error(error));
  }

  onsubmitPassword() {
    const eventData = {...this.eventForm.value};
    const params = new HttpParams().set('info', eventData.password);
    this.http.get<Telegram[]>("https://localhost:44408/telegramintegration/login", {params}).subscribe(result => {
      this.telegram = result;
      console.log(this.telegram)
      if (this.telegram.length > 0 && this.telegram[0].message && this.telegram[0].message.includes("password")) {
        alert("Please Enter a valid password");
      } else if (this.telegram.length > 0 && this.telegram[0].message && this.telegram[0].message.includes("Login Successful")) {
        alert(this.telegram[0].message);
        window.location.href = 'https://localhost:44408/'
      }
    }, error => console.error(error));
  }
}

interface Telegram {
  message: String;
}
