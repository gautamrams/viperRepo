from PIL import Image
import csv
import os


def create_training_set_csv(number_of_training_images, classifier_csv, overlap_increment):
    with open(classifier_csv, mode='w', newline='') as image_matrix:
        matrix_writer = csv.writer(image_matrix, delimiter=',')  # Create a new CSV writer object
        for entry in os.listdir("/root/datascience/original/train/")[:number_of_training_images]:  # Iterate over every file in the regular image directory
            current_image = entry.split(".")[0]  # Remove the ".jpg" file extension, we just want the filename
            print("Addding image " + current_image + " to the test set")

            # Load the regular image into the pix variable using the Image function from the Pillow library
            regular_image = Image.open("/root/datascience/original/train/" + current_image + ".jpg")
            pix = regular_image.load()
            regular_image_width, regular_image_height = regular_image.size

            # Load the facemask into the pix2 variable
            facemask_image = Image.open("/root/datascience/skin/train/" + current_image + "_s.bmp")
            pix2 = facemask_image.load()

            # We want to resize the image dimensions so that the width and height are divisible by 7
            if (regular_image_width % 7) == 0:
                pass
            else:
                regular_image_width = 7 * (regular_image_width // 7)

            if (regular_image_height % 7) == 0:
                pass
            else:
                regular_image_height = 7 * (regular_image_height // 7)
            start_x = 0
            while start_x < regular_image_width - 7:  # Iterate over every pixel on the x axis
                start_y = 0
                while start_y < regular_image_height - 7:  # Iterate over every pixel on the y axis
                    seven_by_seven_grid = []  # Create an empty grid so that we can create a 7x7 grid
                    for pixel_x in range(start_x, start_x + 7):  # Create a grid that is 7 pixels long on the x axis
                        for pixel_y in range(start_y, start_y + 7):  # Create a grid that is 7 pixels long on the y axis
                            for rgb_value in pix[pixel_x, pixel_y]:  # For every pixel in the 7x7 grid extract the RGB value
                                seven_by_seven_grid.append(rgb_value)
                    center_pixel = pix[start_x + 3, start_y + 3]
                    if center_pixel == pix2[start_x + 3, start_y + 3] and center_pixel != (255,255,255):
                        seven_by_seven_grid.append(1)  # If the RGB color is the same in the regular image and the facemask, assign the classifier value of 1
                    else:
                        seven_by_seven_grid.append(0)  # If the RGB color isn't the same in the regular image and the facemask, assign the classifier value of 0
                    matrix_writer.writerow(seven_by_seven_grid)
                    start_y += overlap_increment
                start_x += overlap_increment
        image_matrix.close()  # Close the CSV file
    print("\nCreated a CSV that has a training set of " + str(number_of_training_images) + " images!\n")
