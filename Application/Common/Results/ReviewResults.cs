namespace Application.Common.Results
{
    public static class ReviewResults
    {
        #region Errors
        public static readonly Error InviteAlreadySent = Error.Validation("Review.InviteAlreadySent", "On this booking review invite already sent to the client.");
        public static readonly Error InviteDoesntExists = Error.Validation("Review.InviteDoesntExists", "Review invite doesn't exists.");
        public static readonly Error InviteExpired = Error.Validation("Review.InviteExpired", "Review invitation is expired.");
        public static readonly Error AlreadyReviewed = Error.Validation("Review.AlreadyReviewed", "Already reviewed by client.");
        public static readonly Error NotYourInvite = Error.Validation("Review.NotYourInvite", "This review invite doesn't belong to you.");
        public static readonly Error BookingCreatedForUnauthenticatedClient = Error.Validation("Booking.BookingCreatedForUnauthenticatedClient", "Booking created for unauthenticated client, so can't be reviewd by client.");
        public static readonly Error NotFound = Error.NotFound("Review.NotFound", "Review not found.");
        #endregion
        #region Success
        public static readonly SuccessInfo InvitedCreated = new("Review.InvitedCreated", "Client successfully invited on a review.");
        public static readonly SuccessInfo ReviewCreated = new("Review.ReviewCreated", "Review created successfully.");
        #endregion
    }
}
