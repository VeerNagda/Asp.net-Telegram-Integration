import { Component } from '@angular/core';
import {FormControl, Validators} from "@angular/forms";
import {max} from "rxjs";

@Component({
  selector: 'app-telegram-login',
  templateUrl: './telegram-login.component.html',
  styleUrls: ['./telegram-login.component.css']
})
export class TelegramLoginComponent {

  phone = new FormControl('',[Validators]);
}
