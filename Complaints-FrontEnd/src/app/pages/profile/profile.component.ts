import { DeleteComplainService } from './../../services/DeleteComplain/delete-complain.service';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ContactServeiceService } from '../../services/Contact/contact-serveice.service';
import { Router, RouterModule } from '@angular/router';
import { CaseByUserService } from '../../services/CasesofUser/case-by-user.service';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, FormsModule,FormsModule,RouterModule ],
  templateUrl: './profile.component.html',
  providers:[ContactServeiceService , CaseByUserService ,DeleteComplainService ]
})
export class ProfileComponent {
  firstname: string = '';
  lastname: string = '';
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  gender: number = 1;
  isEditing: boolean = false;
  complaints: any[] = [];
  customerId:string="";
  user = {
    firstname: '',
    lastname: '',
    email: '',
    gender: '',
  };
  length:number= 0;
  complaintIdToDelete: string = '';
  selectedImage: string | ArrayBuffer | null = null;
  isDeleting: boolean = false;

  constructor(private contactservice: ContactServeiceService,private router:Router , private casebyuser: CaseByUserService , private DeleteComplain:DeleteComplainService) {}

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    console.log(token);

    if(token)
    {
      this.contactservice.getUserByToken().subscribe({
        next: (res) => {
          console.log('User Ingo:', res);
          this.user.email = res.data.email,
          localStorage.setItem('email', res.data.email);
          this.user.firstname = res.data.firstname,
          this.user.gender = res.data.gender,
          this.user.lastname = res.data.lastname,
          this.gender = res.data.gender; // 1 or 2
          console.log("res.data.gende: ",res.data.gender);
          console.log(this.user.firstname)
          this.firstname=this.user.firstname;
          this.lastname=this.user.lastname;
          this.email = this.user.email
          console.log("this.firstname: ",this.firstname)
        },
        error: (err) => {
          console.error('Error fetching User Ingo:', err);
        }
      });
      this.casebyuser.getContactIdFromToken().subscribe({
        next: (res) => {
          console.log('User Ingo8888:' , res);
          console.log('User Ingo:', res.data.contactId);
          console.log('User Ingo:', res.data.contactId);
          console.log('User Ingo:', res.data.contactId);
          console.log('User Ingo:', res.data.contactId);
          this.casebyuser.getComplaintsByCustomerId(res.data.contactId).subscribe({
            next: (res) => {
              console.log(res.data);
              this.complaints = res.data;
              console.log(res.data.length)
              this.length=res.data.length;
            },
            error: (err) => {
              console.error('Error fetching complaints: ', err);
            }
          });
        },
        error: (err) => {
          console.error('Error fetching User Ingo: ', err);
        }
      });
    }
  }
  showConfirmPopup = false;
  showSuccessPopup = false;
  selectedComplaint: any = null;

  openModal(complaint: any): void {
    this.selectedComplaint = complaint;
  }

  closeModal(): void {
    this.selectedComplaint = null;
  }

  setComplaintIdToDelete(id: string) {
    this.complaintIdToDelete = id;
    console.log("id: ",id)
    console.log("this.complaintIdToDelete: " , this.complaintIdToDelete)
    this.showConfirmPopup = true;
  }

  onFileSelected(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        this.selectedImage = reader.result;
      };
      reader.readAsDataURL(file);
    }
  }

  deleteImage(): void {
    this.selectedImage = null;
  }

  toggleEditMode() {
    this.isEditing = !this.isEditing;
  }

  confirmDelete() {
    if (!this.complaintIdToDelete) return;
    this.isDeleting = true;
    this.DeleteComplain.Delete(this.complaintIdToDelete).subscribe({
      next: () => {
        this.showSuccessPopup = true;
        this.showConfirmPopup = false;
        this.complaints = this.complaints.filter(c => c.complaintId !== this.complaintIdToDelete);
        setTimeout(() => {
          this.showSuccessPopup = false;
          this.isDeleting = false;
        }, 1000);
      },
      error: err => {
        console.error("Error deleting complaint: ", err);
        this.isDeleting = false;
      }
    });
  }
}
