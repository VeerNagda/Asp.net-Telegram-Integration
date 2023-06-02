import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {HttpClient} from "@angular/common/http";


@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {
  messageForm!: FormGroup;
  public telegram: Telegram = new Telegram();

  constructor(private http: HttpClient, private formBuilder: FormBuilder) {
  }

  ngOnInit() {
    this.messageForm = this.formBuilder.group({
      mobileNumber: ['', [Validators.required, Validators.pattern(/^\+[1-9]\d{1,14}$/)]],
      message: ['', Validators.required]
    });
  }

  onSubmit() {
    const messageData = {...this.messageForm.value};
    this.http.post<Telegram>("https://localhost:44408/telegramintegration/message", messageData).subscribe(result => {
      this.telegram = result;
      if (this.telegram.message.includes('Field_Required'))
        alert('Please fill all the required fields.');
      else if (this.telegram.message.includes("Message Sent"))
        alert('Message sent successfully.');
      else if (this.telegram.message.includes('Please Login Again'))
          alert('Your session has expired. Please login again.');
      else if (this.telegram.message.includes('Retry'))
          alert('Something went wrong. Please try again.');
      else if (this.telegram.message.includes('Enter a valid number'))
          alert('Please enter a valid mobile number.');
      else
        alert(this.telegram.message)
    })
  }
}

class Telegram {
  get message(): String {
    return this._message;
  }

  set message(value: String) {
    this._message = value;
  }
  protected _message: String='';
}
