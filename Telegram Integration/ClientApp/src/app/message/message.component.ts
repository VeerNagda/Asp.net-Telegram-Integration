import { Component } from '@angular/core';
import {FormBuilder} from "@angular/forms";
import {HttpClient, HttpParams} from "@angular/common/http";


@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent {
  mobileNumber: any;
  message: any;

  constructor(private http: HttpClient, private fb: FormBuilder) {}

  onSubmit() {
    this.http.get<Telegram[]>("https://localhost:44408/telegramintegration/message", {params}).subscribe(result{
      
    })
  }
}

interface Telegram {
  message: String;
}
