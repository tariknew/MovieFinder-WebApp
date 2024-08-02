import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ToastrService} from 'ngx-toastr';
import {environment} from '@env/environment';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {catchError, first} from 'rxjs/operators';
import {throwError} from 'rxjs';
import {Router} from '@angular/router';
import {JwtService} from '@auth/services/jwt.services';

@Component({
  selector: 'app-profile-edit-info',
  templateUrl: './profile-edit-info.component.html',
  styleUrls: ['./profile-edit-info.component.css']
})
export class ProfileEditInfoComponent implements OnInit {
  passwordForm: FormGroup;
  emailForm: FormGroup;
  selectedFile: File;

  showAlert = false;

  readerResult: string | ArrayBuffer | null = null;
  imageUrl: string | ArrayBuffer | null = null;
  userStorageData = null;
  userData = null;

  constructor(
    private formBuilder: FormBuilder,
    private toastrService: ToastrService,
    private router: Router,
    private http: HttpClient,
    private jwtService: JwtService
  ) {
  }

  private createForm(): void {
    this.passwordForm = this.formBuilder.group({
      Username: [''],
      currentPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(8)]],
    });
    this.emailForm = this.formBuilder.group({
      Email: ['', Validators.required]
    });
  }

  refreshPage() {
    const currentUrl = this.router.url;
    this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
      this.router.navigateByUrl(currentUrl);
    });
  }

  onPasswordChange() {
    if (this.passwordForm.invalid) {
      return;
    }
    this.toastrService.clear();
    this.changePassword()
      .pipe(first())
      .subscribe(
        () => {
          this.toastrService.success('Password has been changed successfully!');
          this.refreshPage();
        },
        error => {
          this.toastrService.error(error);
        }
      );
  }

  onEmailChange() {
    if (this.emailForm.invalid || !this.emailForm.dirty) {
      return;
    }
    this.toastrService.clear();
    this.changeEmail()
      .pipe(first())
      .subscribe(
        () => {
          this.toastrService.success('Email has been changed successfully!');
          this.refreshPage();
        },
        error => {
          this.toastrService.error(error);
        }
      );
  }

  onPictureChange() {
    if (!this.readerResult) {
      return;
    }
    this.toastrService.clear();
    this.uploadImage(this.readerResult as string)
      .pipe(first())
      .subscribe(
        () => {
          this.toastrService.success('Picture has been changed successfully!');
          this.refreshPage();
        },
        error => {
          this.toastrService.error(error);
        }
      );
  }

  changePassword() {
    // Pozivam ponovo kako bi se dobio 'userData' objekt i iz njega vrijednosti
    this.getUserData(this.userStorageData.IdentityUserId);
    const apiUrl = `${environment.apiUrl}/Profile/ChangePassword`;
    const requestBody = {
      identityUserId: this.userData.identityUserID.toString(),
      currentPassword: this.passwordForm.get('currentPassword').value,
      newPassword: this.passwordForm.get('newPassword').value,
    };
    // Naznačimo da šaljemo JSON podatke na server
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.http.post(apiUrl, requestBody, {headers}).pipe(
      catchError(this.handleError),
    );
  }

  changeEmail() {
    this.getUserData(this.userStorageData.IdentityUserId);
    const apiUrl = `${environment.apiUrl}/Profile/ChangeEmail`;
    const requestBody = {
      identityUserId: this.userData.identityUserID.toString(),
      email: this.emailForm.get('Email').value
    };
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.http.post(apiUrl, requestBody, {headers}).pipe(
      catchError(this.handleError),
    );
  }

  onFileSelected(event: any): void {
    const selectedFile = event.target.files[0];
    if (selectedFile && selectedFile.type.startsWith('image/')) {
      this.selectedFile = selectedFile;
      this.readAndUploadImage();
      this.showAlert = false;
    } else {
      this.showAlert = true;
      // Resetiranje vrijednosti input elementa
      event.target.value = ''; // Ovo će osigurati da se 'change' događaj ponovno okine
    }
  }

  readAndUploadImage(): void {
    const reader = new FileReader();
    reader.onload = () => {
      this.imageUrl = reader.result;
      this.readerResult = reader.result as string;
    };
    reader.readAsDataURL(this.selectedFile);
  }

  uploadImage(result: string) {
    const apiUrl = `${environment.apiUrl}/Profile/ChangeProfilePicture`;
    const requestBody = {
      userID: this.userStorageData.userId,
      profileImageString: result as string
    };
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.http.put(apiUrl, requestBody, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  getUserData(identityUserId: string) {
    const apiUrl = `${environment.apiUrl}/Profile?identityUserId=${identityUserId}`;
    this.http.get<any>(apiUrl)
      .pipe(
        catchError(this.handleError)
      )
      .subscribe(data => {
        this.userData = data;
        this.fillForm(this.userData);
        this.fillPicture(this.userData.profileImage);
      });
  }

  fillPicture(base64Image: string) {
    if (base64Image.length !== 0) {
      this.imageUrl = `data:image/jpeg;base64,${base64Image}`;
    } else {
      this.imageUrl = './assets/img/register-avatar.png';
    }
  }

  fillForm(userData: any): void {
    this.passwordForm.patchValue({
      Username: this.userData.userName
    });
    this.emailForm.patchValue({
      Email: this.userData.email
    });
  }

  closeAlert() {
    this.showAlert = false;
  }

  private handleError(err: HttpErrorResponse) {
    let errorMessage: string;
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `${err.error}`;
    }
    console.error(err);
    return throwError(errorMessage);
  }

  getUser() {
    const storedToken = localStorage.getItem('token');
    const storedUserId = localStorage.getItem('userId');
    if (storedToken) {
      const decodedToken = this.jwtService.getInfoFromToken(storedToken);
      const IdentityUserIdFromToken = decodedToken.sub;
      const usernameFromToken = decodedToken.username;
      // 'JSON.parse' smo koristili kako bi 'userId' koji je tipa 'string' pretvorili u objekt
      this.userStorageData = {
        IdentityUserId: IdentityUserIdFromToken,
        userId: JSON.parse(storedUserId),
        userName: usernameFromToken
      };
      if (this.userStorageData) {
        this.createForm();
      }
    }
  }

  ngOnInit() {
    this.getUser();
    this.getUserData(this.userStorageData.IdentityUserId);
  }
}
