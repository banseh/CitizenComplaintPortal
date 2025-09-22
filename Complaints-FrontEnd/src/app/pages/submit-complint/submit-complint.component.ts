import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CategoryService } from '../../services/Category/category.service';
import { ComplaintService } from '../../services/Complaint/complaint.service';
import { ContactServeiceService } from '../../services/Contact/contact-serveice.service';
import { Router } from '@angular/router';
import { HomeComponent } from '../home/home.component';

@Component({
  selector: 'app-submit-complint',
  imports: [CommonModule, FormsModule],
  templateUrl: './submit-complint.component.html',
  styleUrl: './submit-complint.component.css' ,
  providers: [CategoryService , ComplaintService,ContactServeiceService],
})
export class SubmitComplintComponent {
  complaint = {
    categoryBinding: '',
    name: '',
    description: '',
    nationalId: '',
    email: '',
    location: ''
  }
  user = {
    firstname: '',
    lastname: '',
    email: '',
    gender: '',
    isAnynoums: ''
  };
  isSubmitting: boolean = false;
  TiketNumber:string=" "
  showSuccessPopup = false;
  showErrorPopup = false;
  emailDisabled: boolean = false;
  selectedCategoryId: string = '';
  categories: any[] = [];

  constructor(private categoryService: CategoryService ,private complaintService: ComplaintService,private contactServeiceService:ContactServeiceService , private router:Router) {}

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    console.log(token);
    const savedEmail = localStorage.getItem('email');
    if (savedEmail) {
      this.emailDisabled = true;
      this.complaint.email=savedEmail
    }
    else
    {
      this.emailDisabled = false;
    }

    if(token)
    {
      this.contactServeiceService.getUserByToken().subscribe({
        next: (res) => {
          console.log('User Ingo: ', res);
          this.user.email = res.data.email,
          this.user.firstname = res.data.firstname,
          this.user.gender = res.data.gender,
          this.user.lastname = res.data.lastname,
          this.user.isAnynoums = res.data.isAnynoums
        },
        error: (err) => {
          console.error('Error fetching User Ingo: ', err);
        }
    });
  }

    this.categoryService.getAllCategories().subscribe({
      next: (res) => {
      console.log('API response: ', res.data);
      this.categories = res.data;
      },
      error: (err) => {
        console.error('Error fetching categories: ', err);
      }
    });
  }

  get isPlaceholderSelected(): boolean {
    return !this.complaint.categoryBinding;
  }

  submitComplaint(form: any) {
    if (form.invalid) {
      form.control.markAllAsTouched();
      return;
    }
    this.isSubmitting = true;
    this. complaint = {
      categoryBinding: `/categories(${this.selectedCategoryId})`,
      name: this.complaint.name,
      description:  this.complaint.description,
      nationalId:this.complaint.nationalId,
      email: this.complaint.email,
      location:this.complaint.location,
    };
      console.log(' Complaint submitted: ', this.complaint);
      const token = localStorage.getItem('token');
  if(token)
  {
    this.complaintService.submitComplaint(this.complaint).subscribe({
      next: (res) => {
        console.log('Complaint submitted successfully ', res);
        this.TiketNumber= res.data;
        if (res.status) {
          this.isSubmitting = false;
          this.showSuccessPopup = true;
        } else {
          this.isSubmitting = false;
          this.showErrorPopup = true;
          setTimeout(() => this.showErrorPopup = false, 2000);
        }
        },
        error: (err) => {
          console.error('Failed to submit complaint: ', err);
          this.isSubmitting = false;
          this.showErrorPopup = true;
          setTimeout(() => this.showErrorPopup = false, 2000);
          console.error('Complaint submission failed: ', err);
        }
      });
    }
    else
    {
      this.complaintService.submitGuestComplaint(this.complaint).subscribe({
      next: (res) => {
        console.log('Complaint submitted guest successfully: ', res);
      if (res.status) {
          this.isSubmitting = false;
          this.showSuccessPopup = true;
        this.TiketNumber= res.data;
        } else {
          this.isSubmitting = false;
          this.showErrorPopup = true;
          setTimeout(() => this.showErrorPopup = false, 1000);
        }
        },
        error: (err) => {
          console.error('Failed to submit guest complaint: ', err);
        }
      });
    }
      form.resetForm();
    }

  onCategoryChange()
  {
    console.log('Selected categoryId: ', this.selectedCategoryId);
  }

  closePopups()
  {
    this.showSuccessPopup = false;
    this.showErrorPopup = false;
  }

  pressme()
  {
    this.showSuccessPopup = false;
    this.router.navigate(['/home']);
  }
}





