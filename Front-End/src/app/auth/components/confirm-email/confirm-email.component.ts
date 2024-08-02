import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {ToastrService} from 'ngx-toastr';
import { ConfirmEmailService } from '@auth/services/confirm-email.service';
import {of} from 'rxjs';

@Component({
    selector: 'app-confirm-email',
    templateUrl: './confirm-email.component.html',
    styleUrls: ['./confirm-email.component.css']
})
export class ConfirmEmailComponent implements OnInit {
  confirmationStatus: boolean;

    constructor(private confirmEmailService: ConfirmEmailService, private route: ActivatedRoute, private toastrService: ToastrService) {}

    ngOnInit(): void {
        this.confirmEmail();
    }


    confirmEmail() {
        const token = this.route.snapshot.queryParams.token;
        const userName = this.route.snapshot.queryParams.userName;
        this.confirmEmailService.confirmEmail(userName, token).subscribe(
            (response) => {
                this.confirmationStatus = true;
                return true;
            },
            (error) => {
                this.confirmationStatus = false;
                this.toastrService.error(error);
                return of(false);
            }
        );
    }
}

