﻿using System;

namespace SexyFramework.Drivers
{
	// Token: 0x02000053 RID: 83
	public enum HttpStatusCodes
	{
		// Token: 0x040001DD RID: 477
		HTTP_CONTINUE = 100,
		// Token: 0x040001DE RID: 478
		HTTP_SWITCH_PROTOCOLS,
		// Token: 0x040001DF RID: 479
		HTTP_OK = 200,
		// Token: 0x040001E0 RID: 480
		HTTP_CREATED,
		// Token: 0x040001E1 RID: 481
		HTTP_ACCEPTED,
		// Token: 0x040001E2 RID: 482
		HTTP_PARTIAL,
		// Token: 0x040001E3 RID: 483
		HTTP_NO_CONTENT,
		// Token: 0x040001E4 RID: 484
		HTTP_RESET_CONTENT,
		// Token: 0x040001E5 RID: 485
		HTTP_PARTIAL_CONTENT,
		// Token: 0x040001E6 RID: 486
		HTTP_WEBDAV_MULTI_STATUS,
		// Token: 0x040001E7 RID: 487
		HTTP_AMBIGUOUS = 300,
		// Token: 0x040001E8 RID: 488
		HTTP_MOVED,
		// Token: 0x040001E9 RID: 489
		HTTP_REDIRECT,
		// Token: 0x040001EA RID: 490
		HTTP_REDIRECT_METHOD,
		// Token: 0x040001EB RID: 491
		HTTP_NOT_MODIFIED,
		// Token: 0x040001EC RID: 492
		HTTP_USE_PROXY,
		// Token: 0x040001ED RID: 493
		HTTP_REDIRECT_KEEP_VERB = 307,
		// Token: 0x040001EE RID: 494
		HTTP_BAD_REQUEST = 400,
		// Token: 0x040001EF RID: 495
		HTTP_DENIED,
		// Token: 0x040001F0 RID: 496
		HTTP_PAYMENT_REQ,
		// Token: 0x040001F1 RID: 497
		HTTP_FORBIDDEN,
		// Token: 0x040001F2 RID: 498
		HTTP_NOT_FOUND,
		// Token: 0x040001F3 RID: 499
		HTTP_BAD_METHOD,
		// Token: 0x040001F4 RID: 500
		HTTP_NONE_ACCEPTABLE,
		// Token: 0x040001F5 RID: 501
		HTTP_PROXY_AUTH_REQ,
		// Token: 0x040001F6 RID: 502
		HTTP_REQUEST_TIMEOUT,
		// Token: 0x040001F7 RID: 503
		HTTP_CONFLICT,
		// Token: 0x040001F8 RID: 504
		HTTP_GONE,
		// Token: 0x040001F9 RID: 505
		HTTP_LENGTH_REQUIRED,
		// Token: 0x040001FA RID: 506
		HTTP_PRECOND_FAILED,
		// Token: 0x040001FB RID: 507
		HTTP_REQUEST_TOO_LARGE,
		// Token: 0x040001FC RID: 508
		HTTP_URI_TOO_LONG,
		// Token: 0x040001FD RID: 509
		HTTP_UNSUPPORTED_MEDIA,
		// Token: 0x040001FE RID: 510
		HTTP_RETRY_WITH = 449,
		// Token: 0x040001FF RID: 511
		HTTP_SERVER_ERROR = 500,
		// Token: 0x04000200 RID: 512
		HTTP_NOT_SUPPORTED,
		// Token: 0x04000201 RID: 513
		HTTP_BAD_GATEWAY,
		// Token: 0x04000202 RID: 514
		HTTP_SERVICE_UNAVAIL,
		// Token: 0x04000203 RID: 515
		HTTP_GATEWAY_TIMEOUT,
		// Token: 0x04000204 RID: 516
		HTTP_VERSION_NOT_SUP
	}
}